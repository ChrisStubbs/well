﻿namespace PH.Well.Repositories.Read
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Dapper;
    using Domain.ValueObjects;

    public class RouteReadRepository : IRouteReadRepository
    {
        private readonly ILogger logger;
        private readonly IDapperReadProxy dapperReadProxy;

        public RouteReadRepository(ILogger logger, IDapperReadProxy dapperReadProxy)
        {
            this.logger = logger;
            this.dapperReadProxy = dapperReadProxy;
        }


        public IEnumerable<Route> GetAllRoutes(string username)
        {
            var routes = new List<Route>();
            dapperReadProxy.WithStoredProcedure(StoredProcedures.RoutesGetAll)
                    .AddParameter("username", username, DbType.String)
                    .QueryMultiple(x => routes = GetReadRoutesFromGrid(x));

            return routes;
        }

        public List<Route> GetReadRoutesFromGrid(SqlMapper.GridReader grid)
        {
            var readRoutes = grid.Read<Route>().ToList();
            var assignees = grid.Read<RouteAssignees>().ToList();
            var jobIds = grid.Read<RouteJob>().ToList();

            foreach (var route in readRoutes)
            {
                route.Assignees = assignees.Where(x => x.RouteId == route.Id).ToList();
                route.JobIds = jobIds.Where(x => x.RouteId == route.Id).Select(x => x.JobId).ToList();
            }

            return readRoutes;
        }

    }
}
