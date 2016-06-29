namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Dapper;
    using Domain;

    public class StopRepository : DapperRepository<Stop, int>, IStopRepository
    {
        public StopRepository(ILogger logger, IWellDapperProxy dapperProxy) : base(logger, dapperProxy)
        {
        }

        public IEnumerable<Stop> GetStopByRouteHeaderId(int routeHeaderId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("routeHeaderId", routeHeaderId, DbType.Int32);

            return dapperProxy.Query<Stop>(StoredProcedures.StopsGetByRouteHeaderId, parameters);
        }

    }
}
