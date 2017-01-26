namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;

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
                var stops = stopRepository.GetStopByRouteHeaderId(routeHeader.Id);

                foreach (var stop in stops)
                {
                    stop.Jobs = new List<Job>(jobRepository.GetByStopId(stop.Id));
                }

                routeHeader.Stops = stops.ToList();
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

        public RouteHeader GetByNumberDateBranch(string routeNumber, DateTime routeDate, int branchId)
        {
            return
                this.dapperProxy.WithStoredProcedure(StoredProcedures.RouteheaderGetByNumberDateBranch)
                    .AddParameter("RouteNumber", routeNumber, DbType.String)
                    .AddParameter("RouteDate", routeDate, DbType.DateTime)
                    .AddParameter("BranchId", branchId, DbType.Int32)
                    .Query<RouteHeader>()
                    .FirstOrDefault();
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

        protected override void SaveNew(RouteHeader entity)
        {
            entity.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeaderInsert)
                .AddParameter("CompanyId", entity.CompanyId, DbType.Int32)
                .AddParameter("RouteNumber", entity.RouteNumber, DbType.String)
                .AddParameter("RouteOwnerId", entity.RouteOwnerId, DbType.Int32)
                .AddParameter("RouteDate", entity.RouteDate, DbType.DateTime)
                .AddParameter("DriverName", entity.DriverName, DbType.String)
                .AddParameter("StartDepotCode", entity.StartDepot, DbType.Int32)
                .AddParameter("PlannedStops", entity.PlannedStops, DbType.Int16)
                .AddParameter("ActualStopsCompleted", entity.ActualStopsCompleted, DbType.Int16)
                .AddParameter("RoutesId", entity.RoutesId, DbType.Int32)
                .AddParameter("RouteStatusId", (int)entity.RouteStatus, DbType.Int16)
                .AddParameter("RoutePerformanceStatusId", (int)entity.RoutePerformanceStatusId, DbType.Int16)
                .AddParameter("LastRouteUpdate", entity.LastRouteUpdate, DbType.DateTime)
                .AddParameter("AuthByPass", entity.AuthByPass, DbType.Int32)
                .AddParameter("NonAuthByPass", entity.NonAuthByPass, DbType.Int32)
                .AddParameter("ShortDeliveries ", entity.ShortDeliveries, DbType.Int32)
                .AddParameter("DamagesRejected", entity.DamagesRejected, DbType.Int32)
                .AddParameter("DamagesAccepted", entity.DamagesAccepted, DbType.Int32)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("CreatedDate", entity.DateCreated, DbType.DateTime)
                .AddParameter("UpdatedDate", entity.DateUpdated, DbType.DateTime)
                .Query<int>().FirstOrDefault();
        }

        protected override void UpdateExisting(RouteHeader entity)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeaderUpdate)
                .AddParameter("Id", entity.Id, DbType.Int32)
                .AddParameter("RouteStatusId", (int)entity.RouteStatus, DbType.Int16)
                .AddParameter("RoutePerformanceStatusId", (int)entity.RoutePerformanceStatusId, DbType.Int16)
                .AddParameter("LastRouteUpdate", entity.LastRouteUpdate, DbType.DateTime)
                .AddParameter("AuthByPass", entity.AuthByPass, DbType.Int32)
                .AddParameter("NonAuthByPass", entity.NonAuthByPass, DbType.Int32)
                .AddParameter("ShortDeliveries ", entity.ShortDeliveries, DbType.Int32)
                .AddParameter("DamagesRejected", entity.DamagesRejected, DbType.Int32)
                .AddParameter("DamagesAccepted", entity.DamagesAccepted, DbType.Int32)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("UpdatedDate", entity.DateUpdated, DbType.DateTime).Execute();
        }

        public RouteHeader GetRouteHeaderByRoute(string routeNumber, DateTime? routeDate)
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
