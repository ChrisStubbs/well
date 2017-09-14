using PH.Shared.Well.Data.EF;
using PH.Well.Domain.Constants;

namespace PH.Well.Repositories.Read
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Dapper;
    using Domain.Enums;
    using Domain.ValueObjects;

    public class RouteReadRepository : IRouteReadRepository
    {
        private readonly ILogger logger;
        private readonly IDapperReadProxy dapperReadProxy;
        private readonly WellEntities wellEntities;

        public RouteReadRepository(ILogger logger, IDapperReadProxy dapperReadProxy, WellEntities wellEntities)
        {
            this.logger = logger;
            this.dapperReadProxy = dapperReadProxy;
            this.wellEntities = wellEntities;
        }

        public IEnumerable<Route> GetAllRoutesForBranch(int branchId, string username)
        {
            var branch = wellEntities.UserBranch
                .Where(x => x.User.IdentityName == username && x.Branch.Id == branchId)
                .Select(x => new
                {
                    x.Branch.Id,
                    x.Branch.Name
                })
                .FirstOrDefault();

            if (branch != null)
            {
                var routesWithNoGRNView = this.wellEntities.RoutesWithNoGRNView
                    .Where(p => p.BranchId == branchId)
                    .ToDictionary(k => k.Id, v => v.NoGRNButNeeds.Value);

                var routesWithUnresolvedActionView = this.wellEntities.RoutesWithUnresolvedActionView
                    .Where(p => p.BranchId == branchId)
                    .ToDictionary(k => k.Id, v => v.HasNotDefinedDeliveryAction.Value);

                var routesWithPendingSubmitionsView = this.wellEntities.RoutesWithPendingSubmitionsView
                    .Where(p => p.BranchId == branchId)
                    .ToDictionary(k => k.Id, v => v.PendingSubmission.Value);

                var jobs = wellEntities.RouteHeader
                    .Where(x => x.RouteOwnerId == branch.Id && x.DateDeleted == null)
                    .SelectMany(p => p.Stop
                        .Where(s => s.DateDeleted == null)
                        .SelectMany(s => s.Job
                            .Where(j => j.DateDeleted == null && j.JobTypeCode != "DEL-DOC" &&
                                        j.JobTypeCode != "UPL-SAN")
                            .Select(uj => new
                            {
                                Users = uj.UserJob.Select(a => a.User.Name),
                                Route = p.Id,
                                Stop = s.Id
                            })
                        )
                    ).ToList()
                    //group the data by route and stop, because the total unallocated will be displayed not as total jobs but total stops
                    .GroupBy(p => new {p.Route, p.Stop})
                    //select distinct users inside the group
                    .Select(p =>
                    {
                        var distinctUsers = p.SelectMany(data => data.Users).Distinct().ToList();

                        if (distinctUsers == null || distinctUsers.Count() == 0)
                        {
                            return new
                            {
                                Users = new List<string> {"Unallocated"},
                                p.Key.Route
                            };
                        }

                        return new
                        {
                            Users = distinctUsers,
                            p.Key.Route
                        };
                    })
                    .GroupBy(p => p.Route)
                    .ToDictionary(k => k.Key, v => v.ToList());

                var exceptionTotals = wellEntities.ExceptionTotalsPerRoute
                    .Where(p => p.BranchId == branchId)
                    .ToDictionary(k => k.Routeid, v => new {v.WithExceptions, v.WithOutExceptions});

                var routeHeaders = wellEntities.RouteHeader
                    .Where(x => x.RouteOwnerId == branch.Id && x.DateDeleted == null)
                    .Select(x => new
                    {
                        RouteId = x.Id,
                        BranchId = x.RouteOwnerId,
                        BranchName = branch.Name,
                        RouteNumber = x.RouteNumber,
                        RouteDate = x.RouteDate,
                        StopCount = x.Stop.Where(p => p.DateDeleted == null).Count(),
                        x.WellStatus,
                        DriverName = x.DriverName,
                        JobIds = x.Stop.SelectMany(y => y.Job.Where(z => z.DateDeleted == null).Select(a => a.Id))
                    })
                    .ToList();

                Func<int, List<Assignee>> getAssignees = routeId =>
                {
                    if (jobs.ContainsKey(routeId))
                    {
                        return jobs[routeId]
                            .SelectMany(p => p.Users)
                            .Select(p => new Assignee()
                            {
                                RouteId = routeId,
                                Name = p
                            }).ToList();
                    }

                    return null;
                };

                return routeHeaders
                    .Select(item =>
                    {
                        var routeWellStatus = (item.WellStatus.HasValue)
                            ? (WellStatus) item.WellStatus
                            : WellStatus.Unknown;

                        var hasNotDefinedDeliveryAction = routesWithUnresolvedActionView.ContainsKey(item.RouteId) ? routesWithUnresolvedActionView[item.RouteId] : false;
                        var noGRNButNeeds = routesWithNoGRNView.ContainsKey(item.RouteId) ? routesWithNoGRNView[item.RouteId] : false;
                        var pendingSubmission = routesWithPendingSubmitionsView.ContainsKey(item.RouteId) ? routesWithPendingSubmitionsView[item.RouteId] : false;

                        var route = new Route()
                        {
                            Id = item.RouteId,
                            BranchId = item.BranchId,
                            BranchName = item.BranchName,
                            RouteNumber = item.RouteNumber,
                            RouteDate = item.RouteDate.Value,
                            StopCount = item.StopCount,
                            ExceptionCount = exceptionTotals.ContainsKey(item.RouteId)
                                ? exceptionTotals[item.RouteId].WithExceptions.Value
                                : 0,
                            CleanCount = exceptionTotals.ContainsKey(item.RouteId)
                                ? exceptionTotals[item.RouteId].WithOutExceptions.Value
                                : 0,
                            RouteStatusId = (int) routeWellStatus,
                            RouteStatus = routeWellStatus.Description(),

                            Assignees = getAssignees(item.RouteId),
                            JobIssueType =
                                (hasNotDefinedDeliveryAction ? JobIssueType.ActionRequired : JobIssueType.All) |
                                (noGRNButNeeds ? JobIssueType.MissingGRN : JobIssueType.All) |
                                (pendingSubmission ? JobIssueType.PendingSubmission : JobIssueType.All),
                            JobIds = item.JobIds.ToList(),
                            DriverName = item.DriverName
                        };

                        return route;
                    })
                    .ToList();
            }

            return new List<Route>();
        }
        
        public List<Route> GetReadRoutesFromGrid(SqlMapper.GridReader grid)
        {
            var readRoutes = grid.Read<Route>().ToList();
            var assignees = grid.Read<Assignee>().ToList();
            var jobIds = grid.Read<RouteJob>().ToList();

            foreach (var route in readRoutes)
            {
                route.Assignees = assignees.Where(x => x.RouteId == route.Id).ToList();
                route.JobIds = jobIds.Where(x => x.RouteId == route.Id && x.JobType != JobType.Documents).Select(x => x.JobId).ToList();
            }

            return readRoutes;
        }

    }
}
