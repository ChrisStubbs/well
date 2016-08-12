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
    using Domain.Enums;

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
            var routeHeaders = dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeadersGet)
              .AddParameter("UserName", this.CurrentUser, DbType.String)
              .Query<RouteHeader>();

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
                .AddParameter("Username", this.CurrentUser, DbType.String).Query<int>().FirstOrDefault();

            return this.GetById(id);

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

        public Routes GetByFilename(string filename)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.RoutesCheckDuplicate)
                    .AddParameter("FileName", filename, DbType.String).Query<Routes>().FirstOrDefault();
        }

        public RouteHeader RouteHeaderCreateOrUpdate(RouteHeader routeHeader)
        {
           
            var id = this.dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeaderCreateOrUpdate)
                .AddParameter("Id", routeHeader.Id, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String)
                .AddParameter("CompanyId", routeHeader.CompanyID, DbType.Int32)
                .AddParameter("RouteNumber", routeHeader.RouteNumber, DbType.String)
                .AddParameter("RouteDate", routeHeader.RouteDate, DbType.DateTime)
                .AddParameter("DriverName", routeHeader.DriverName, DbType.String)
                .AddParameter("VehicleReg", routeHeader.VehicleReg, DbType.String)
                .AddParameter("StartDepotCode", routeHeader.StartDepot, DbType.Int32)
                .AddParameter("PlannedRouteStartTime", routeHeader.PlannedRouteStartTime, DbType.String)
                .AddParameter("PlannedRouteFinishTime", routeHeader.PlannedRouteFinishTime, DbType.String)
                .AddParameter("PlannedDistance", routeHeader.PlannedDistance, DbType.Decimal)
                .AddParameter("PlannedTravelTime", routeHeader.PlannedTravelTime, DbType.String)
                .AddParameter("PlannedStops", routeHeader.PlannedStops, DbType.Int16)
                .AddParameter("ActualStopsCompleted", routeHeader.PlannedStops, DbType.Int16)
                .AddParameter("RoutesId", routeHeader.RoutesId, DbType.Int32)
                .AddParameter("RouteStatusId", routeHeader.RouteStatus = (int)routeHeader.RouteStatus == 0 ? (int)RouteStatusCode.Notdef : routeHeader.RouteStatus, DbType.Int16)
                .AddParameter("RoutePerformanceStatusId", routeHeader.RoutePerformanceStatusId == 0 ? (int)RoutePerformanceStatusCode.Notdef : routeHeader.RoutePerformanceStatusId, DbType.Int16)
                .AddParameter("LastRouteUpdate", routeHeader.LastRouteUpdate, DbType.DateTime)
                .AddParameter("AuthByPass", routeHeader.AuthByPass, DbType.Int32)
                .AddParameter("NonAuthByPass", routeHeader.NonAuthByPass, DbType.Int32)
                .AddParameter("ShortDeliveries ", routeHeader.ShortDeliveries, DbType.Int32)
                .AddParameter("DamagesRejected", routeHeader.DamagesRejected, DbType.Int32)
                .AddParameter("DamagesAccepted", routeHeader.DamagesAccepted, DbType.Int32)
                .AddParameter("NotRequired", routeHeader.NotRequired, DbType.Int32)
                .AddParameter("Depot", routeHeader.EpodDepot, DbType.Int32).Query<int>().FirstOrDefault();

            return this.GetRouteHeaderById(id);

        }

        public void AddRouteHeaderAttributes(Domain.Attribute attribute)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeaderAttributeCreateOrUpdate)
                .AddParameter("Id", attribute.Id, DbType.Int32)
                .AddParameter("Code", attribute.Code, DbType.String)
                .AddParameter("Value", attribute.Value1, DbType.String)
                .AddParameter("RouteHeaderId", attribute.AttributeId, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String).Query<int>();
        }

        public RouteHeader GetRouteHeaderByRouteNumberAndDate(string routeNumber, DateTime routeDate)
        {
            var routeImportId =
                dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeaderGetByRouteNumberAndDate)
                    .AddParameter("RouteNumber", routeNumber, DbType.String)
                    .AddParameter("RouteDate", routeDate, DbType.DateTime)
                    .Query<RouteHeader>()
                    .FirstOrDefault();

            return routeImportId;
        }

        public IEnumerable<RouteAttributeException>  GetRouteAttributeException()
        {

            return dapperProxy.WithStoredProcedure(StoredProcedures.RouteAttributesGetExceptions)
                .Query<RouteAttributeException>();
        }







    }
}
