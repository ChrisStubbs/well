namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;

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

        public Stop GetById(int id)
        {
            var stop =
               dapperProxy.WithStoredProcedure(StoredProcedures.StopGetById)
                   .AddParameter("Id", id, DbType.Int32)
                   .Query<Stop>()
                   .FirstOrDefault();

            return stop;
        }

        public Stop StopCreateOrUpdate(Stop stop)
        {
            var stopStatusId = stop.StopStatusCodeId == 0 ? (int) StopStatus.Notdef : stop.StopStatusCodeId;
            var stopPerformanceStatusId = stop.StopPerformanceStatusCodeId == 0 ? (int)PerformanceStatus.Notdef : stop.StopPerformanceStatusCodeId;
            var stopByPassReasonId = stop.ByPassReasonId == 0 ? (int)ByPassReasons.Notdef : stop.ByPassReasonId;

            var id = this.dapperProxy.WithStoredProcedure(StoredProcedures.StopsCreateOrUpdate)
                .AddParameter("Id", stop.Id, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String)
                .AddParameter("PlannedStopNumber", stop.PlannedStopNumber, DbType.Int32)
                .AddParameter("PlannedArriveTime", stop.PlannedArrivalTime, DbType.String)
                .AddParameter("PlannedDepartTime", stop.PlannedDepartTime, DbType.String)
                .AddParameter("RouteHeaderId", stop.RouteHeaderId, DbType.Int32)
                .AddParameter("DropId", stop.DropId, DbType.String)
                .AddParameter("LocatiodId", stop.LocationId, DbType.String)
                .AddParameter("DeliveryDate", stop.DeliveryDate, DbType.DateTime)
                .AddParameter("SpecialInstructions", stop.SpecialInstructions, DbType.String)
                .AddParameter("StartWindow", stop.StartWindow, DbType.String)
                .AddParameter("EndWindow", stop.EndWindow, DbType.String)
                .AddParameter("TextField1", stop.TextField1, DbType.String)
                .AddParameter("TextField2", stop.TextField2, DbType.String)
                .AddParameter("TextField3", stop.TextField3, DbType.String)
                .AddParameter("TextField4", stop.TextField4, DbType.String)
                .AddParameter("StopStatusId", stopStatusId, DbType.Int16)
                .AddParameter("StopPerformanceStatusId", stopPerformanceStatusId, DbType.Int16)
                .AddParameter("ByPassReasonId", stopByPassReasonId, DbType.Int16).Query<int>().FirstOrDefault();

            return this.GetById(id);

        }

        public void AddStopAttributes(Attribute attribute)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.StopAttributeCreateOrUpdate)
                .AddParameter("Id", attribute.Id, DbType.Int32)
                .AddParameter("Code", attribute.Code, DbType.String)
                .AddParameter("Value", attribute.Value1, DbType.String)
                .AddParameter("StopId", attribute.AttributeId, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String).Query<int>();
        }

        public void StopAccountCreateOrUpdate(Account account)
        {
            var dropAndDrive = account.IsDropAndDrive == "True" ? true : false;

            this.dapperProxy.WithStoredProcedure(StoredProcedures.StopAccountCreateOrUpdate)
                .AddParameter("Id", account.Id, DbType.Int32)
                .AddParameter("Code", account.Code, DbType.String)
                .AddParameter("Username", this.CurrentUser, DbType.String)
                .AddParameter("AccountTypeCode", account.AccountTypeCode, DbType.String)
                .AddParameter("DepotId", account.DepotId, DbType.Int32)
                .AddParameter("Name", account.Name, DbType.String)
                .AddParameter("Address1", account.Address1, DbType.String)
                .AddParameter("Address2", account.Address2, DbType.String)
                .AddParameter("PostCode", account.PostCode, DbType.String)
                .AddParameter("ContactName", account.ContactName, DbType.String)
                .AddParameter("ContactNumber", account.ContactNumber, DbType.String)
                .AddParameter("ContactNumber2", account.ContactNumber2, DbType.String)
                .AddParameter("ContactEmailAddress", account.ContactEmailAddress, DbType.String)
                .AddParameter("StartWindow", account.StartWindow, DbType.String)
                .AddParameter("EndWindow", account.EndWindow, DbType.String)
                .AddParameter("Latitude", account.Longitude, DbType.Double)
                .AddParameter("Longitude", account.Latitude, DbType.Double)
                .AddParameter("IsDropAndDrive", dropAndDrive, DbType.Boolean)
                .AddParameter("StopId", account.StopId, DbType.Int32).Query<int>().FirstOrDefault();

        }

    }
}
