namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
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
            var routeHeaders = this.dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeadersGet)
                                            .Query<RouteHeader>().ToArray();

            foreach (var routeHeader in routeHeaders)
            {
                var stops = stopRepository.GetStopByRouteHeaderId(routeHeader.Id).ToList();
                routeHeader.Stops = new Collection<Stop>(stops);
            }

            return routeHeaders;
        }


    }
}
