namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using Common.Contracts;
    using Contracts;
    using Domain;

    public class StopRepository : DapperRepository<Stop, int>, IStopRepository
    {
        public StopRepository(ILogger logger, IWellDapperProxy dapperProxy) : base(logger, dapperProxy)
        {
        }

        public IEnumerable<Stop> GetStopByRouteHeaderId(int routeHeaderId)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.StopsGetByRouteHeaderId)
                                    .AddParameter("routeHeaderId", routeHeaderId, DbType.Int32)
                                    .Query<Stop>();
        }

    }
}
