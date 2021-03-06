﻿namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Common.Extensions;
    using Contracts;
    using Dapper;
    using Domain;
    using Domain.ValueObjects;
    using static PH.Well.Domain.Mappers.AutoMapperConfig;

    public class RouteHeaderRepository : DapperRepository<RouteHeader, int>, IRouteHeaderRepository
    {
        public RouteHeaderRepository(ILogger logger, IWellDapperProxy dapperProxy, IUserNameProvider userNameProvider)
            : base(logger, dapperProxy, userNameProvider)
        {
        }

        /// <summary>
        /// Get RouteHeaders with its related stops preloaded
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RouteHeader> GetRouteHeaders()
        {
            IEnumerable<RouteHeader> routes = new List<RouteHeader>();
            var routeHeaders =
                dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeaderGetAll)
                    .AddParameter("UserName", this.CurrentUser, DbType.String)
                    .QueryMultiple(x => routes = GetStopsByRoute(x));
            return routes;
        }

        private IEnumerable<RouteHeader> GetStopsByRoute(SqlMapper.GridReader gridReader)
        {
            var routeHeaders = gridReader.Read<RouteHeader>().ToList();
            var stops = gridReader.Read<Stop>().ToList();
            var jobs = gridReader.Read<Job>().ToList();
            foreach (var routeHeader in routeHeaders)
            {
                routeHeader.Stops = Mapper.Map<List<Stop>, List<StopDTO>>(stops.Where(x => x.RouteHeaderId == routeHeader.Id).ToList());
                
                foreach (var stop in routeHeader.Stops)
                {
                    stop.Jobs = Mapper.Map<List<Job>, List<JobDTO>>(jobs.Where(x => x.StopId == stop.Id).ToList());
                }
            }
            return routeHeaders;
        }

        public IEnumerable<RouteHeader> GetRouteHeadersGetByRoutesId(int routesId)
        {
            var routeHeaders =
                dapperProxy.WithStoredProcedure(StoredProcedures.RouteheaderGetByRouteId)
                    .AddParameter("RouteId", routesId, DbType.Int32)
                    .Query<RouteHeader>();

            return routeHeaders;
        }

        public IList<GetByNumberDateBranchResult> GetByNumberDateBranch(IList<GetByNumberDateBranchFilter> filter)
        {
            return
                this.dapperProxy.WithStoredProcedure(StoredProcedures.RouteheaderGetByNumberDateBranch)
                    .AddParameter("filter", filter.ToDataTables(), DbType.Object)
                    .Query<GetByNumberDateBranchResult>()
                    .ToList();
        }

        public void DeleteRouteHeaderWithNoStops()
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.DeleteRouteHeaderWithNoStops)
                .AddParameter("UpdatedBy", CurrentUser, DbType.String)
                .Execute();
        }

        public IEnumerable<HolidayExceptionsWell> HolidayExceptionGet()
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.HolidayExceptionGet).Query<HolidayExceptionsWell>();
        }

        public IEnumerable<Routes> GetRoutes()
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.RoutesGet).Query<Routes>();
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
                .AddParameter("RouteStatusCode", entity.RouteStatusCode, DbType.String)
                .AddParameter("RouteStatusDescription", entity.RouteStatusDescription, DbType.String)
                .AddParameter("PerformanceStatusCode", entity.PerformanceStatusCode, DbType.String)
                .AddParameter("PerformanceStatusDescription", entity.PerformanceStatusDescription, DbType.String)
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
                .AddParameter("RouteStatusCode", entity.RouteStatusCode, DbType.String)
                .AddParameter("RouteStatusDescription", entity.RouteStatusDescription, DbType.String)
                .AddParameter("PerformanceStatusCode", entity.PerformanceStatusCode, DbType.String)
                .AddParameter("PerformanceStatusDescription", entity.PerformanceStatusDescription, DbType.String)
                .AddParameter("LastRouteUpdate", entity.LastRouteUpdate, DbType.DateTime)
                .AddParameter("AuthByPass", entity.AuthByPass, DbType.Int32)
                .AddParameter("NonAuthByPass", entity.NonAuthByPass, DbType.Int32)
                .AddParameter("ShortDeliveries ", entity.ShortDeliveries, DbType.Int32)
                .AddParameter("DamagesRejected", entity.DamagesRejected, DbType.Int32)
                .AddParameter("DamagesAccepted", entity.DamagesAccepted, DbType.Int32)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("DriverName", entity.DriverName, DbType.String)
                .AddParameter("UpdatedDate", entity.DateUpdated, DbType.DateTime)
                .AddParameter("StartDepotCode", entity.StartDepot, DbType.Int32)
                .AddParameter("WellStatus", (int) entity.RouteWellStatus, DbType.Int32)
                .AddParameter("PlannedStops", entity.PlannedStops, DbType.Int16).Execute();
        }

        public RouteHeader GetRouteHeaderByRoute(int branchId, string routeNumber, DateTime? routeDate)
        {
            return
                dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeaderGetByBranchRouteNumberAndDate)
                    .AddParameter("BranchId", branchId, DbType.Int32)
                    .AddParameter("RouteNumber", routeNumber, DbType.String)
                    .AddParameter("RouteDate", routeDate, DbType.DateTime)
                    .Query<RouteHeader>()
                    .FirstOrDefault();
        }

        public IEnumerable<RouteAttributeException> GetRouteAttributeException()
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

        public void UpdateWellStatus(RouteHeader routeHeader)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.RouteHeaderUpdateWellStatus)
                .AddParameter("Id", routeHeader.Id, DbType.Int32)
                .AddParameter("WellStatusId", (int) routeHeader.RouteWellStatus, DbType.Int16)
                .Execute();
        }

        public void UpdateFieldsFromImported(RouteHeaderFromImportedFile newData)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.UpdateFieldsFromImported)
                .AddParameter("Id", newData.Id, DbType.Int32)
                .AddParameter("PlannedStops", newData.PlannedStops, DbType.Int16)
                .AddParameter("RouteDate", newData.RouteDate, DbType.DateTime)
                .AddParameter("RouteNumber", newData.RouteNumber, DbType.String)
                .AddParameter("RouteOwnerId", newData.RouteOwnerId, DbType.Int32)
                .AddParameter("StartDepot", newData.StartDepot, DbType.Int32)
            .Execute();
        }
    }
}
