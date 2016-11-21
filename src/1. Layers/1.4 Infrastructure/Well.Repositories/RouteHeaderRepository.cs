namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;

    using PH.Well.Domain.Enums;

    public class RouteHeaderRepository : DapperRepository<RouteHeader, int> , IRouteHeaderRepository
    {
        private readonly IStopRepository stopRepository;
        private readonly IJobRepository jobRepository;

        public RouteHeaderRepository(ILogger logger,
            IWellDapperProxy dapperProxy,
            IStopRepository stopRepository,
            IJobRepository jobRepository) : base(logger, dapperProxy)
        {
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
        }

        public IEnumerable<RouteHeader> GetRouteHeaders()
        {
            var routeHeaders = dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeadersGet)
              .AddParameter("UserName", this.CurrentUser, DbType.String)
              .Query<RouteHeader>();

            foreach (var routeHeader in routeHeaders)
            {
                var stops = stopRepository.GetStopByRouteHeaderId(routeHeader.Id).ToList();
                foreach (var stop in stops)
                {
                    stop.Jobs = new Collection<Job>(jobRepository.GetByStopId(stop.Id).ToList());
                }
                routeHeader.Stops = new Collection<Stop>(stops);
            }
        
            return routeHeaders;
        }

        public IEnumerable<RouteHeader> GetRouteHeadersGetByRoutesId(int routesId)
        {
            var routeHeaders = dapperProxy.WithStoredProcedure(StoredProcedures.RouteheaderGetByRouteId)
              .AddParameter("RouteId", routesId, DbType.Int32)
              .Query<RouteHeader>();

            return routeHeaders;
        }

        public IEnumerable<HolidayExceptions> HolidayExceptionGet()
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.HolidayExceptionGet)
              .Query<HolidayExceptions>();
        }

        public IEnumerable<Routes> GetRoutes()
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.RoutesGet)
              .Query<Routes>();          
        }

        public IEnumerable<RouteHeader> GetRouteHeadersForDelete()
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeadersGetForDelete)
              .Query<RouteHeader>();
        }

        public Routes Create(Routes route)
        {
            route.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.RouteInsert)
                .AddParameter("Filename", route.FileName, DbType.String)
                .AddParameter("Username", this.CurrentUser, DbType.String).Query<int>().FirstOrDefault();

            return route;
        }

        public Routes GetById(int id)
        {
            var routeImport =
                dapperProxy.WithStoredProcedure(StoredProcedures.RoutesGetById)
                    .AddParameter("Id", id, DbType.Int32)
                    .Query<Routes>()
                    .FirstOrDefault();

            return routeImport;
        }

        public RouteHeader GetRouteHeaderById(int id)
        {
            var routeImport =
                dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeaderGetById)
                    .AddParameter("Id", id, DbType.Int32)
                    .Query<RouteHeader>()
                    .FirstOrDefault();

            return routeImport;
        }

        public bool FileAlreadyLoaded(string filename)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.RoutesCheckDuplicate)
                    .AddParameter("FileName", filename, DbType.String).Query<Routes>().FirstOrDefault() != null;
        }

        public void RouteHeaderCreateOrUpdate(RouteHeader routeHeader)
        {
           routeHeader.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeaderCreateOrUpdate)
                .AddParameter("Id", routeHeader.Id, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String)
                .AddParameter("CompanyId", routeHeader.CompanyId, DbType.Int32)
                .AddParameter("RouteNumber", routeHeader.RouteNumber, DbType.String)
                .AddParameter("RouteDate", routeHeader.RouteDate, DbType.String)
                .AddParameter("DriverName", routeHeader.DriverName, DbType.String)
                .AddParameter("StartDepotCode", routeHeader.StartDepot, DbType.Int32)
                .AddParameter("PlannedStops", routeHeader.PlannedStops, DbType.Int16)
                .AddParameter("ActualStopsCompleted", routeHeader.PlannedStops, DbType.Int16)
                .AddParameter("RoutesId", routeHeader.RoutesId, DbType.Int32)
                .AddParameter("RouteStatusId", routeHeader.RouteStatus = (int)routeHeader.RouteStatus == 0 ? (int)RouteStatusCode.Notdef : routeHeader.RouteStatus, DbType.Int16)
                .AddParameter("RoutePerformanceStatusId", routeHeader.RoutePerformanceStatusId == 0 ? (int)RoutePerformanceStatusCode.Notdef : routeHeader.RoutePerformanceStatusId, DbType.Int16)
                .AddParameter("LastRouteUpdate", routeHeader.LastRouteUpdate, DbType.String)
                .AddParameter("AuthByPass", routeHeader.AuthByPass, DbType.Int32)
                .AddParameter("NonAuthByPass", routeHeader.NonAuthByPass, DbType.Int32)
                .AddParameter("ShortDeliveries ", routeHeader.ShortDeliveries, DbType.Int32)
                .AddParameter("DamagesRejected", routeHeader.DamagesRejected, DbType.Int32)
                .AddParameter("DamagesAccepted", routeHeader.DamagesAccepted, DbType.Int32)
                .AddParameter("NotRequired", routeHeader.NotRequired, DbType.Int32)
                .AddParameter("Depot", routeHeader.EpodDepot, DbType.Int32)
                .AddParameter("RouteOwnerId", routeHeader.RouteOwnerId, DbType.Int32)
                .Query<int>().FirstOrDefault();
        }

        public RouteHeader GetRouteHeaderByTransportOrderReference(string routeNumber, DateTime? routeDate)
        {
            return
                dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeaderGetByRouteNumberAndDate)
                    .AddParameter("RouteNumber", routeNumber, DbType.String)
                    .AddParameter("RouteDate", routeDate, DbType.DateTime)
                    .Query<RouteHeader>()
                    .FirstOrDefault();
        }

        public IEnumerable<RouteAttributeException>  GetRouteAttributeException()
        {

            return dapperProxy.WithStoredProcedure(StoredProcedures.RouteAttributesGetExceptions)
                .Query<RouteAttributeException>();
        }

        public void RoutesDeleteById(int id)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.RoutesDeleteById)
                .AddParameter("RoutesId", id, DbType.Int32)
                .Execute();
        }

        public void DeleteRouteHeaderById(int id)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeaderDeleteById)
                .AddParameter("RouteheaderId", id, DbType.Int32)
                .Execute();
        }
    }
}
