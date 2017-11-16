using System.Diagnostics;
using System.Transactions;
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
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }))
            {
                var userBranch = wellEntities.UserBranch.Any(x => x.User.IdentityName == username && x.Branch.Id == branchId);
                if (userBranch)
                {
                    var routeHeaders = wellEntities.RouteHeader
                        .Where(x => x.RouteOwnerId == branchId)
                        .Select(x => new
                        {
                            RouteId = x.Id,
                            BranchId = x.RouteOwnerId,
                            BranchName = x.Branch.Name,
                            RouteNumber = x.RouteNumber,
                            RouteDate = x.RouteDate,
                            StopCount = x.Stop.Count(s=> s.DateUpdated != null),
                            x.WellStatus,
                            DriverName = x.DriverName,
                            x.ExceptionCount,
                            x.CleanCount,
                            x.HasNotDefinedDeliveryAction,
                            x.NoGRNButNeeds,
                            x.PendingSubmission,
                            JobIds = x.Stop.SelectMany(y => y.Job.Select(a => a.Id))
                        })
                        .ToList();

                    var routeIds = routeHeaders.Select(x => x.RouteId).ToList();

                    var jobs = wellEntities.Stop
                        .Where(s => routeIds.Contains(s.RouteHeaderId))
                        .SelectMany(s => s.Job
                            .Select(uj => new
                            {
                                Users = uj.UserJob.Select(a => a.User.Name),
                                Route = s.RouteHeaderId,
                                Stop = s.Id
                            })
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
                    
                    var result = routeHeaders
                        .Select(item =>
                        {
                            var routeWellStatus = (item.WellStatus.HasValue)
                                ? (WellStatus) item.WellStatus
                                : WellStatus.Unknown;
                            
                            return new Route()
                            {
                                Id = item.RouteId,
                                BranchId = item.BranchId,
                                BranchName = item.BranchName,
                                RouteNumber = item.RouteNumber,
                                RouteDate = item.RouteDate.Value,
                                StopCount = item.StopCount,
                                ExceptionCount = item.ExceptionCount.GetValueOrDefault(),
                                CleanCount = item.CleanCount.GetValueOrDefault(),
                                RouteStatusId = (int) routeWellStatus,
                                RouteStatus = routeWellStatus.Description(),

                                Assignees = getAssignees(item.RouteId),
                                /*future routes have no issue type*/
                                JobIssueType = item.RouteDate.Value.Date > DateTime.Now.Date ? JobIssueType.All : 
                                    (item.HasNotDefinedDeliveryAction.GetValueOrDefault()
                                        ? JobIssueType.ActionRequired
                                        : JobIssueType.All) |
                                    (item.NoGRNButNeeds.GetValueOrDefault()
                                        ? JobIssueType.MissingGRN
                                        : JobIssueType.All) |
                                    (item.PendingSubmission.GetValueOrDefault()
                                        ? JobIssueType.PendingSubmission
                                        : JobIssueType.All),
                                JobIds = item.JobIds.ToList(),
                                DriverName = item.DriverName ?? string.Empty
                            };
                        })
                        .ToList();
                    scope.Complete();
                    return result;
                }

                scope.Complete();
                logger.LogDebug($"No branch found for the User '{username}'");
                return new List<Route>();
            }
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
