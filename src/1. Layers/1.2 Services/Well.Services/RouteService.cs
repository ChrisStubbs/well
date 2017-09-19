using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using PH.Well.Domain;
using PH.Well.Domain.Enums;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories.Contracts;
using PH.Well.Services.Contracts;

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

        #endregion Private fields

        #region Constructors
        public RouteService(
            IRouteHeaderRepository routeHeaderRepository,
            IWellStatusAggregator wellStatusAggregator,
            IStopRepository stopRepository,
            INotificationRepository notificationRepository,
            IUserNameProvider userNameProvider)
        {
            this.routeHeaderRepository = routeHeaderRepository;
            this.wellStatusAggregator = wellStatusAggregator;
            this.stopRepository = stopRepository;
            this.notificationRepository = notificationRepository;
            this.userNameProvider = userNameProvider;
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
            RouteHeader routeHeader = routeHeaderRepository.GetRouteHeaderById(routeId);
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
                        Source =  userNameProvider.GetUserName()
                    };

                    notificationRepository.SaveNotification(notification);
                }

                return routeHeader;
            }
            throw new ArgumentException($"RouteHeader not found id : {routeId}", nameof(routeId));
        }

        #endregion Public methods
    }
}
