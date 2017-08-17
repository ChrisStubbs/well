using PH.Shared.Well.Data.EF;

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
            var routes = new List<Route>();
            var branch = wellEntities.UserBranch.Where(x => x.User.IdentityName == username).Select(x => x.Branch).FirstOrDefault(x => x.Id == branchId);
            if (branch != null)
            {
                var routeHeaders = wellEntities.RouteHeader.Where(x => x.RouteOwnerId == branch.Id).Select(x => new
                {
                    RouteId = x.Id,
                    BranchId = x.RouteOwnerId,
                    Assignees = x.Stop.SelectMany(y => y.Job.SelectMany(z => z.UserJob.Select(a => a.User.Name))).Distinct(),
                    BranchName = branch.Name,
                    RouteNumber = x.RouteNumber,
                    RouteDate = x.RouteDate,
                    StopCount = x.Stop.Count,
                    RouteStatusCode = x.RouteStatusCode,
                    RouteStatusDesc = x.RouteStatusDescription,
                    BypassJobCount = x.Stop.Count(y => y.Job.All(z => z.JobTypeCode != "UPL-SAN"
                                                                        && z.JobTypeCode != "DEL-DOC"
                                                                        && z.JobTypeCode != "NOTDEF"
                                                                        //&& z.ResolutionStatusId > 1
                                                                        && z.JobStatusId == (byte)JobStatus.Bypassed)),
                    ExceptionCount = x.Stop.Count(y => y.Job.Any(z => z.JobTypeCode != "UPL-SAN"
                                                                      && z.JobTypeCode != "DEL-DOC"
                                                                      && z.JobTypeCode != "NOTDEF"
                                                                      && z.ResolutionStatusId > 1
                                                                      && z.Activity.LineItem.Any(a => a.LineItemAction.Any()))),
                    CleanCount = x.Stop.Count(y => y.Job.Any(z => z.JobTypeCode != "UPL-SAN"
                                                                      && z.JobTypeCode != "DEL-DOC"
                                                                      && z.JobTypeCode != "NOTDEF"
                                                                      && z.ResolutionStatusId > 1
                                                                      && z.Activity.LineItem.Any(a => !a.LineItemAction.Any()))),
                    DriverName = x.DriverName,
                    JobIds = x.Stop.SelectMany(y => y.Job.Where(z => z.JobTypeCode != "UPL-SAN"
                                                                        && z.JobTypeCode != "DEL-DOC"
                                                                        && z.JobTypeCode != "NOTDEF").Select(a => a.Id))
                }).ToList();

                foreach (var routeHeader in routeHeaders)
                {
                    var route = new Route()
                    {
                        Id = routeHeader.RouteId,
                        BranchId = routeHeader.BranchId,
                        BranchName = routeHeader.BranchName,
                        RouteNumber = routeHeader.RouteNumber,
                        RouteDate = routeHeader.RouteDate.Value,
                        StopCount = routeHeader.StopCount,
                        ExceptionCount = routeHeader.ExceptionCount,
                        CleanCount = routeHeader.CleanCount,
                        RouteStatus = routeHeader.RouteStatusDesc,
                        RouteStatusId =
                            GetWellStatus(routeHeader.RouteStatusCode, routeHeader.BypassJobCount,
                                routeHeader.JobIds.Count()),
                        Assignees = routeHeader.Assignees.Select(x=>new Assignee()
                        {
                            RouteId = routeHeader.RouteId,
                            Name = x
                        }).ToList()
                    };

                    routes.Add(route);
            }
        }
            return routes;
        }

    private int GetWellStatus(string routeStatusCode, int bypassJobCount, int jobCount)
    {
        if (bypassJobCount == jobCount)
        {
            return 4;
        }
        switch (routeStatusCode)
        {
            case "NDEPA":
                return 1;
            case "INPRO":
                return 2;
            case "COMPL":
                return 3;
            default:
                return 1;
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
