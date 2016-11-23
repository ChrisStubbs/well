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

        public Stop GetByJobId(int jobId)
        {
            var stop =
               dapperProxy.WithStoredProcedure(StoredProcedures.StopGetByJobId)
                   .AddParameter("JobId", jobId, DbType.Int32)
                   .Query<Stop>()
                   .FirstOrDefault();

            return stop;
        }

        protected override void SaveNew(Stop entity)
        {
            entity.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.StopInsert)
                .AddParameter("TransportOrderReference", entity.TransportOrderReference, DbType.String)
                .AddParameter("PlannedStopNumber", entity.PlannedStopNumber, DbType.Int32)
                .AddParameter("RouteHeaderCode", entity.RouteHeaderCode, DbType.String)
                .AddParameter("RouteHeaderId", entity.RouteHeaderId, DbType.Int32)
                .AddParameter("DropId", entity.DropId, DbType.String)
                .AddParameter("LocationId", entity.LocationId, DbType.String)
                .AddParameter("DeliveryDate", entity.DeliveryDate, DbType.DateTime)
                .AddParameter("ShellActionIndicator", entity.ShellActionIndicator, DbType.String)
                .AddParameter("CustomerShopReference", entity.CustomerShopReference, DbType.String)
                .AddParameter("AllowOvers", entity.AllowOvers == "True", DbType.Boolean)
                .AddParameter("CustUnatt", entity.CustUnatt == "True", DbType.Boolean)
                .AddParameter("PHUnatt", entity.PHUnatt == "True", DbType.Boolean)
                .AddParameter("StopStatusId", entity.StopStatusCodeId, DbType.Int16)
                .AddParameter("StopPerformanceStatusId", entity.StopPerformanceStatusCodeId, DbType.Int16)
                .AddParameter("ByPassReasonId", entity.ByPassReasonId, DbType.Int16)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("CreatedDate", entity.DateCreated, DbType.DateTime)
                .AddParameter("UpdatedDate", entity.DateUpdated, DbType.DateTime)
                .AddParameter("ActualPaymentCash", entity.ActualPaymentCash, DbType.Decimal)
                .AddParameter("ActualPaymentCheque", entity.ActualPaymentCheque, DbType.Decimal)
                .AddParameter("ActualPaymentCard", entity.ActualPaymentCard, DbType.Decimal).Query<int>().FirstOrDefault();
        }

        public void StopCreateOrUpdate(Stop stop)
        {
            // TODO removed in new refactor keep eye on it
            var stopStatusId = stop.StopStatusCodeId == 0 ? (int)StopStatus.Notdef : stop.StopStatusCodeId;
            var stopPerformanceStatusId = stop.StopPerformanceStatusCodeId == 0 ? (int)PerformanceStatus.Notdef : stop.StopPerformanceStatusCodeId;
            var stopByPassReasonId = stop.ByPassReasonId == 0 ? (int)ByPassReasons.Notdef : stop.ByPassReasonId;

            var id = this.dapperProxy.WithStoredProcedure(StoredProcedures.StopsCreateOrUpdate)
                .AddParameter("Id", stop.Id, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String)
                .AddParameter("TransportOrderReference", stop.TransportOrderReference, DbType.String)
                .AddParameter("PlannedStopNumber", stop.PlannedStopNumber, DbType.Int32)
                .AddParameter("RouteHeaderCode", stop.RouteHeaderCode, DbType.String)
                .AddParameter("RouteHeaderId", stop.RouteHeaderId, DbType.Int32)
                .AddParameter("DropId", stop.DropId, DbType.String)
                .AddParameter("LocationId", stop.LocationId, DbType.String)
                .AddParameter("DeliveryDate", stop.DeliveryDate, DbType.DateTime)
                .AddParameter("ShellActionIndicator", stop.ShellActionIndicator, DbType.String)
                .AddParameter("CustomerShopReference", stop.CustomerShopReference, DbType.String)
                .AddParameter("AllowOvers",  stop.AllowOvers == "True", DbType.Boolean)
                .AddParameter("CustUnatt", stop.CustUnatt == "True", DbType.Boolean)
                .AddParameter("PHUnatt", stop.PHUnatt == "True", DbType.Boolean)
                .AddParameter("StopStatusId", stopStatusId, DbType.Int16)
                .AddParameter("StopPerformanceStatusId", stopPerformanceStatusId, DbType.Int16)
                .AddParameter("ByPassReasonId", stopByPassReasonId, DbType.Int16)
                .AddParameter("ActualPaymentCash", stop.ActualPaymentCash, DbType.Decimal)
                .AddParameter("ActualPaymentCheque", stop.ActualPaymentCheque, DbType.Decimal)
                .AddParameter("ActualPaymentCard", stop.ActualPaymentCard, DbType.Decimal)
                .Query<int>().FirstOrDefault();

            stop.Id = id;
        }
        
        public void StopAccountCreateOrUpdate(Account account)
        {
            account.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.StopAccountCreateOrUpdate)
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
                .AddParameter("StopId", account.StopId, DbType.Int32).Query<int>().FirstOrDefault();
        }

        protected override void UpdateExisting(Stop entity)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.StopUpdate)
                .AddParameter("Id", entity.Id, DbType.Int32)
                .AddParameter("StopStatusCodeId", (int)entity.StopStatusCodeId, DbType.Int16)
                .AddParameter("StopPerformanceStatusCodeId", (int)entity.StopPerformanceStatusCodeId, DbType.Int16)
                .AddParameter("ByPassReasonId", entity.ByPassReasonId, DbType.Int32)
                .AddParameter("PlannedStopNumber", entity.PlannedStopNumber, DbType.String)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("UpdatedDate", entity.DateUpdated, DbType.DateTime).Execute();
        }

        public Stop GetByTransportOrderReference(string transportOrderReference)
        {
            return
               dapperProxy.WithStoredProcedure(StoredProcedures.StopGetByTransportOrderReference)
                   .AddParameter("TransportOrderReference", transportOrderReference, DbType.String)
                   .Query<Stop>()
                   .FirstOrDefault();
        }

        public Stop GetByOrderUpdateDetails(string transportOrderReference)
        {
            var stop =
               dapperProxy.WithStoredProcedure(StoredProcedures.StopGetByOrderUpdateDetails)
                   .AddParameter("transportOrderReference", transportOrderReference, DbType.String)
                   .Query<Stop>()
                   .FirstOrDefault();

            return stop;
        }

        public void DeleteStopById(int id)
        {
            AccountDeleteByStopId(id);

            dapperProxy.WithStoredProcedure(StoredProcedures.StopDeleteById)
                .AddParameter("Id", id, DbType.Int32)
                .Execute();
        }

        public void DeleteStopByTransportOrderReference(string transportOrderReference)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.DeleteStopByTransportOrderReference)
                .AddParameter("TransportOrderReference", transportOrderReference, DbType.String)
                .Execute();
        }

        private void AccountDeleteByStopId(int stopId)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.AccountDeleteByStopId)
                .AddParameter("StopId", stopId, DbType.Int32)
                .Execute();
        }
    }
}
