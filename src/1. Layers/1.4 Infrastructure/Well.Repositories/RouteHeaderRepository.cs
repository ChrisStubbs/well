

namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Dapper;
    using Domain;
  
    public class RouteHeaderRepository : DapperRepository<RouteHeader, int> , IRouteHeaderRepository
    {

        private readonly IStopRepository stopRepository;

        public RouteHeaderRepository(ILogger logger, IWellDapperProxy dapperProxy, IStopRepository stopRepository)
            : base(logger, dapperProxy)
        {
            this.stopRepository = stopRepository;
        }


        public IEnumerable<RouteHeader> GetRouteHeaders()
        {
            var routeHeaders = dapperProxy.Query<RouteHeader>(StoredProcedures.RouteHeadersGet, parameters: null);

            foreach (var routeHeader in routeHeaders)
            {
                var stops = stopRepository.GetStopByRouteHeaderId(routeHeader.Id).ToList();
                routeHeader.Stops = new Collection<Stop>(stops);
            }

            return routeHeaders;
        }


    }
}
