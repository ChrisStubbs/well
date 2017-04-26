namespace PH.Well.Repositories.Read
{
    using System.Collections.Generic;
    using System.Data;
    using Common.Contracts;
    using Contracts;
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
            return 
                dapperReadProxy.WithStoredProcedure(StoredProcedures.ReadRouteGetAll)
                    .AddParameter("username", username, DbType.String)
                    .Query<ReadRoute>();
        }

    }
}
