using PH.Shared.Well.Data.EF;
using PH.Well.Domain.Constants;

namespace PH.Well.Repositories.Read
{
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
                .Select(p => new { p.Id, Value = p.NoGRNButNeeds, Type = "a" });

                var routesWithUnresolvedActionView = this.wellEntities.RoutesWithUnresolvedActionView
                    .Select(p => new { p.Id, Value = p.HasNotDefinedDeliveryAction, Type = "b" });

                var routesWithPendingSubmitionsView = this.wellEntities.RoutesWithPendingSubmitionsView
                    .Select(p => new { p.Id, Value = p.PendingSubmission, Type = "c" });

                var viewsValues = routesWithUnresolvedActionView
                    .Union(routesWithNoGRNView)
                    .Union(routesWithPendingSubmitionsView)
                    .GroupBy(p => p.Id)
                    .Select(p => new
                    {
                        Id = p.Key,
                        NoGRNButNeeds = p.Where(v => v.Type == "a").Select(v => v.Value).FirstOrDefault() ?? false,
                        HasNotDefinedDeliveryAction = p.Where(v => v.Type == "b").Select(v => v.Value).FirstOrDefault() ?? false,
                        PendingSubmission = p.Where(v => v.Type == "c").Select(v => v.Value).FirstOrDefault() ?? false,
                    });

                var routeHeaders = wellEntities.RouteHeader
                    .Where(x => x.RouteOwnerId == branch.Id && x.DateDeleted == null)
                    .GroupJoin(viewsValues,
                        p => p.Id,
                        v => v.Id,
                        (p, v) => v
                                    .Select(r => new
                                    {
                                        Route = p,
                                        NoGRNButNeeds = r.NoGRNButNeeds,
                                        HasNotDefinedDeliveryAction = r.HasNotDefinedDeliveryAction,
                                        PendingSubmission = r.PendingSubmission
                                    })
                                    .DefaultIfEmpty(new
                                    {
                                        Route = p,
                                        NoGRNButNeeds = false,
                                        HasNotDefinedDeliveryAction = false,
                                        PendingSubmission = false
                                    })
                    )
                    .SelectMany(p => p)
                    .Select(x => new
                    {
                        x.NoGRNButNeeds,
                        x.HasNotDefinedDeliveryAction,
                        x.PendingSubmission,
                        RouteId = x.Route.Id,
                        BranchId = x.Route.RouteOwnerId,
                        Assignees = x.Route.Stop.SelectMany(y => y.Job.SelectMany(z => z.UserJob.Select(a => a.User.Name))).Distinct(),
                        BranchName = branch.Name,
                        RouteNumber = x.Route.RouteNumber,
                        RouteDate = x.Route.RouteDate,
                        StopCount = x.Route.Stop.Count,
                        RouteStatusCode = x.Route.RouteStatusCode,
                        RouteStatusDesc = x.Route.RouteStatusDescription,
                        BypassJobCount = x.Route.Stop.Count(y => y.Job.All(z => z.JobTypeCode != "UPL-SAN"
                                                                            && z.JobTypeCode != "DEL-DOC"
                                                                            && z.JobTypeCode != "NOTDEF"
                                                                            && z.JobStatusId == (byte)JobStatus.Bypassed)),
                        ExceptionCount = x.Route.Stop.Count(y => y.Job.Any(z => z.JobTypeCode != "UPL-SAN"
                                                                          && z.JobTypeCode != "DEL-DOC"
                                                                          && z.JobTypeCode != "NOTDEF"
                                                                          && z.ResolutionStatusId > 1
                                                                          && z.Activity.LineItem.Any(a => a.LineItemAction.Any()))),
                        CleanCount = x.Route.Stop.Count(y => y.Job.Any(z => z.JobTypeCode != "UPL-SAN"
                                                                          && z.JobTypeCode != "DEL-DOC"
                                                                          && z.JobTypeCode != "NOTDEF"
                                                                          && z.ResolutionStatusId > 1
                                                                          && z.Activity.LineItem.Any(a => !a.LineItemAction.Any()))),
                        DriverName = x.Route.DriverName,
                        JobIds = x.Route.Stop.SelectMany(y => y.Job.Where(z => z.JobTypeCode != "UPL-SAN"
                                                                            && z.JobTypeCode != "DEL-DOC"
                                                                            && z.JobTypeCode != "NOTDEF").Select(a => a.Id))
                    })
                    .ToList();

                return routeHeaders.Select(item => new Route()
                {
                    Id = item.RouteId,
                    BranchId = item.BranchId,
                    BranchName = item.BranchName,
                    RouteNumber = item.RouteNumber,
                    RouteDate = item.RouteDate.Value,
                    StopCount = item.StopCount,
                    ExceptionCount = item.ExceptionCount,
                    CleanCount = item.CleanCount,
                    RouteStatus = item.RouteStatusDesc,
                    RouteStatusId =
                        GetWellStatus(item.RouteStatusCode, item.BypassJobCount,
                            item.JobIds.Count()),
                    Assignees = item.Assignees.Select(x => new Assignee()
                    {
                        RouteId = item.RouteId,
                        Name = x
                    }).ToList(),
                    JobIssueType =
                        (item.HasNotDefinedDeliveryAction ? JobIssueType.ActionRequired : JobIssueType.All) |
                        (item.NoGRNButNeeds ? JobIssueType.MissingGRN : JobIssueType.All) |
                        (item.PendingSubmission ? JobIssueType.PendingSubmission : JobIssueType.All),
                    JobIds = item.JobIds.ToList()
                })
                .ToList();
            }

            return new List<Route>();
        }

        private int GetWellStatus(string routeStatusCode, int bypassJobCount, int jobCount)
        {
            if (bypassJobCount == jobCount)
            {
                return (int)WellStatus.Bypassed;
            }
            switch (routeStatusCode)
            {
                case RouteStatusCode.NotDeparted:
                    return (int)WellStatus.Planned;
                case RouteStatusCode.InProgress:
                    return (int)WellStatus.RouteInProgress;
                case RouteStatusCode.Completed:
                    return (int)WellStatus.Complete;
                default:
                    return (int)WellStatus.Planned;
            }
        }

        //public IEnumerable<Route> GetAllRoutesForBranch(int branchId, string username)
        //{
        //    var routes = new List<Route>();
        //    dapperReadProxy.WithStoredProcedure(StoredProcedures.RoutesGetAllForBranch)
        //            .AddParameter("BranchId", branchId, DbType.Int32)
        //            .AddParameter("username", username, DbType.String)
        //            .QueryMultiple(x => routes = GetReadRoutesFromGrid(x));

        //    return routes;
        //}

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
