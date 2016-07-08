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
        public Routes CreateOrUpdate(Routes routes)
        {
            var parameters = new DynamicParameters();

            parameters.Add("Id", routes.Id, DbType.Int32);
            parameters.Add("Filename", routes.FileName, DbType.String);
            parameters.Add("Username", this.CurrentUser, DbType.String);

            var id = this.dapperProxy.Query<int>(StoredProcedures.RoutesCreateOrUpdate, parameters).FirstOrDefault();

            return this.GetById(id);
        }

        public Routes GetById(int id)
        {
            var parameters = new DynamicParameters();

            parameters.Add("Id", id, DbType.Int32);
            var routeImport = dapperProxy.Query<Routes>(StoredProcedures.RoutesGetById, parameters).FirstOrDefault();
            return routeImport;
        }

        public Routes GetByFilename(string filename)
        {
            var parameters = new DynamicParameters();

            parameters.Add("FileName", filename, DbType.String);
            return dapperProxy.Query<Routes>(StoredProcedures.RoutesCheckDuplicate, parameters).FirstOrDefault();
        }


    }
}
