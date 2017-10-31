using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PH.Shared.Well.Data.EF;
using PH.Well.Domain.Enums;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories.Contracts;
using PH.Well.Services.Contracts;
using ExceptionType = PH.Well.Domain.Enums.ExceptionType;
using JobResolutionStatus = PH.Well.Domain.JobResolutionStatus;
using Notification = PH.Well.Domain.Notification;
using RouteHeader = PH.Well.Domain.RouteHeader;
using WellStatus = PH.Well.Domain.Enums.WellStatus;

namespace PH.Well.Services
{
    using Common.Contracts;
    using Domain.Extensions;

    public class RouteService : IRouteService
    {
        #region Private fields
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IWellStatusAggregator wellStatusAggregator;
        private readonly IStopRepository stopRepository;
        private readonly INotificationRepository notificationRepository;
        private readonly IUserNameProvider userNameProvider;
        private readonly WellEntities wellEntities;

        #endregion Private fields

        #region Constructors
        public RouteService(
            IRouteHeaderRepository routeHeaderRepository,
            IWellStatusAggregator wellStatusAggregator,
            IStopRepository stopRepository,
            INotificationRepository notificationRepository,
            IUserNameProvider userNameProvider,
            WellEntities wellEntities)
        {
            this.routeHeaderRepository = routeHeaderRepository;
            this.wellStatusAggregator = wellStatusAggregator;
            this.stopRepository = stopRepository;
            this.notificationRepository = notificationRepository;
            this.userNameProvider = userNameProvider;
            this.wellEntities = wellEntities;
        }
        #endregion Constructors

        #region Public methods
        public RouteHeader ComputeWellStatus(int routeId)
        {
            RouteHeader routeHeader = routeHeaderRepository.GetRouteHeaderById(routeId);
            if (routeHeader != null)
            {
                return ComputeWellStatus(routeHeader);
            }
            throw new ArgumentException($"RouteHeader not found id : {routeId}", nameof(routeId));
        }

        public RouteHeader ComputeWellStatus(RouteHeader route)
        {
            // Compute new route status from all its active stops
            WellStatus[] wellStatuses;
            if (!route.Stops.Any())
            {
                wellStatuses = stopRepository.GetStopByRouteHeaderId(route.Id).Select(x => x.WellStatus).ToArray();
            }
            else
            {
                wellStatuses = route.Stops.Select(x => x.WellStatus).ToArray();
            }

            var newWellStatus = wellStatusAggregator.Aggregate(wellStatuses.ToArray());

            if (route.RouteWellStatus != newWellStatus)
            {
                // Save the status change back to the repository
                route.RouteWellStatus = newWellStatus;
                routeHeaderRepository.UpdateWellStatus(route);
            }

            return route;
        }

        public RouteHeader ComputeWellStatusAndNotifyIfChangedFromCompleted(int routeId)
        {
            var routeHeader = routeHeaderRepository.GetRouteHeaderById(routeId);
            if (routeHeader != null)
            {
                var existingStatus = routeHeader.RouteWellStatus;

                routeHeader = ComputeWellStatus(routeHeader);

                if (existingStatus == WellStatus.Complete && existingStatus != routeHeader.RouteWellStatus)
                {
                    var branch = ((Domain.Enums.Branch)routeHeader.RouteOwnerId).ToString();
                    var notification = new Notification
                    {
                        Branch = branch,
                        ErrorMessage = $"Route status changed from Complete to {routeHeader.RouteWellStatus.Description()}. " +
                                       $"Branch: {branch}, " +
                                       $"Date: {routeHeader.RouteDate.ToShortDateString()} " +
                                       $"Route No: {routeHeader.RouteNumber}. " +
                                       $"Please check the route for amended stops that require completion",
                        Source = userNameProvider.GetUserName()
                    };

                    notificationRepository.SaveNotification(notification);
                }

                return routeHeader;
            }
            throw new ArgumentException($"RouteHeader not found id : {routeId}", nameof(routeId));
        }

        public void UpdateRouteStatistics(int branchId)
        {
            var routes = wellEntities.RouteHeader.Where(x => x.Branch.Id == branchId).ToList();
            var noGRNButNeeds = wellEntities.RoutesWithNoGRNView.Where(p => p.BranchId== branchId).ToDictionary(x=> x.Id);
            var hasNotDefinedDeliveryAction = wellEntities.RoutesWithUnresolvedActionView.Where(p => p.BranchId == branchId).ToDictionary(x => x.Id);
            var pendingSubmission = wellEntities.RoutesWithPendingSubmitionsView.Where(p => p.BranchId == branchId).ToDictionary(x => x.Id);

            var routesExceptionsCount = wellEntities.RouteHeader.Where(x => x.Branch.Id == branchId).Select(x => new
            {
                x.Id,
                ExceptionCount =
                (byte) x.Stop.Count(s => s.Job.Where(j => j.ResolutionStatusId > 1)
                    .Any(j => j.LineItem.Any(
                        l => l.LineItemAction.Any(la => la.ExceptionTypeId != (int) ExceptionType.Uplifted)))),

                CleanCount =
                (byte) x.Stop.Count(s => s.Job.Where(j => j.ResolutionStatusId > 1)
                    .All(j => j.LineItem.All(
                        l => l.LineItemAction.All(la => la.ExceptionTypeId == (int) ExceptionType.Uplifted))))
            }).ToDictionary(x => x.Id);


            foreach (var routeHeader in routes)
            {
                routeHeader.NoGRNButNeeds = noGRNButNeeds.ContainsKey(routeHeader.Id)
                    ? noGRNButNeeds[routeHeader.Id].NoGRNButNeeds.GetValueOrDefault()
                    : false;

                routeHeader.HasNotDefinedDeliveryAction =
                    hasNotDefinedDeliveryAction.ContainsKey(routeHeader.Id)
                        ? hasNotDefinedDeliveryAction[routeHeader.Id].HasNotDefinedDeliveryAction
                            .GetValueOrDefault()
                        : false;

                routeHeader.PendingSubmission = pendingSubmission.ContainsKey(routeHeader.Id)
                    ? pendingSubmission[routeHeader.Id].PendingSubmission.GetValueOrDefault()
                    : false;

                var routeExceptions = routesExceptionsCount[routeHeader.Id];

                routeHeader.ExceptionCount = routeExceptions.ExceptionCount;
                routeHeader.CleanCount = routeExceptions.CleanCount;
            }

            // Update routes
            wellEntities.SaveChanges();

        }

        #endregion Public methods
    }
}
