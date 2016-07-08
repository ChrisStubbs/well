namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Dapper;
    using Domain;

    public class RouteHeaderRepository : DapperRepository<RouteHeader, int>, IRouteHeaderRepository
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
            var id = this.dapperProxy.WithStoredProcedure(StoredProcedures.RoutesCreateOrUpdate)
                .AddParameter("Id", routes.Id, DbType.Int32)
                .AddParameter("Filename", routes.FileName, DbType.String)
                .AddParameter("Username", this.CurrentUser, DbType.String)
                .Query<int>().SingleOrDefault();

            return this.GetById(id);
        }

        public Routes GetById(int id)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.RoutesGetById)
                        .AddParameter("Id", id, DbType.Int32)
                        .Query<Routes>().SingleOrDefault();
        }

        public Routes GetByFilename(string filename)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.RoutesCheckDuplicate)
                        .AddParameter("FileName", filename, DbType.String)
                        .Query<Routes>().SingleOrDefault();
        }


    }
}
