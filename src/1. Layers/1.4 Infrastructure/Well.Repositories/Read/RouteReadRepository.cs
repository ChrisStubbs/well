namespace PH.Well.Repositories.Read
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


        public IEnumerable<ReadRoute> GetAllRoutes(string username)
        {
            var routes = new List<ReadRoute>();
            dapperReadProxy.WithStoredProcedure(StoredProcedures.ReadRouteGetAll)
                    .AddParameter("username", username, DbType.String)
                    .QueryMultiple(x => routes = GetReadRoutesFromGrid(x));

            return routes;
        }

        public List<ReadRoute> GetReadRoutesFromGrid(SqlMapper.GridReader grid)
        {
            var readRoutes = grid.Read<ReadRoute>().ToList();
            var assignees = grid.Read<ReadRouteAssignees>().ToList();

            foreach (var route in readRoutes)
            {
                route.Assignees = assignees.Where(x => x.RouteId == route.Id).ToList();
            }

            return readRoutes;
        }

    }
}
