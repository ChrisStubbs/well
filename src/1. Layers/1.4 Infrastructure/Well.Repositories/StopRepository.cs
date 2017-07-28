namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Common.Extensions;
    using Contracts;
    using Dapper;
    using Domain;

    public class StopRepository : DapperRepository<Stop, int>, IStopRepository
    {
        public StopRepository(ILogger logger, IWellDapperProxy dapperProxy, IUserNameProvider userNameProvider)
            : base(logger, dapperProxy, userNameProvider)
        {
        }

        public IEnumerable<Stop> GetStopByRouteHeaderId(int routeHeaderId)
        {
            var stopIds = this.dapperProxy.WithStoredProcedure(StoredProcedures.StopsGetByRouteHeaderId)
                                    .AddParameter("routeHeaderId", routeHeaderId, DbType.Int32)
                                    .Query<int>();

            return this.GetByIds(stopIds);
        }

        public IList<Stop> GetByTransportOrderReferences(IList<string> transportOrderReferences)
        {
            var stopIds = this.dapperProxy.WithStoredProcedure(StoredProcedures.StopGetByOrderUpdateDetails)
                .AddParameter("transportOrderReferences", transportOrderReferences, DbType.String)
                .Query<int>();

            return this.GetByIds(stopIds);
        }
        
        public Stop GetByJobDetails(string picklist, string account)
        {
            var stopIds =
                dapperProxy.WithStoredProcedure(StoredProcedures.StopGetByJob)
                    .AddParameter("Picklist", picklist, DbType.String)
                    .AddParameter("Account", account, DbType.String)
                    .Query<int>();

            return GetByIds(stopIds).FirstOrDefault();
        }

        public IList<Stop> GetByIds(IEnumerable<int> stopIds)
        {
            IList<Stop> stops = new List<Stop>();

            this.dapperProxy.WithStoredProcedure(StoredProcedures.StopsGetByIds)
                .AddParameter("Ids", stopIds.ToList().ToIntDataTables("Ids"), DbType.Object)
                .QueryMultiple(x => stops = GetFromGrid(x));

            return stops;
        }

        private IList<Stop> GetFromGrid(SqlMapper.GridReader gridReader)
        {
            var stops = gridReader.Read<Stop>().ToList();
            var accounts = gridReader.Read<Account>().ToList();
            stops.ForEach(s => s.Account = accounts.FirstOrDefault(a => a.StopId == s.Id));
            return stops;
        }

        public Stop GetById(int id)
        {
            return GetByIds(new[] { id }).FirstOrDefault();
        }

        public Stop GetByJobId(int jobId)
        {
            var stopIds =
               dapperProxy.WithStoredProcedure(StoredProcedures.StopGetByJobId)
                   .AddParameter("JobId", jobId, DbType.Int32)
                   .Query<int>();

            return GetByIds(stopIds).FirstOrDefault();
        }

        protected override void SaveNew(Stop entity)
        {
            entity.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.StopInsert)
                .AddParameter("TransportOrderReference", entity.TransportOrderReference, DbType.String)
                .AddParameter("PlannedStopNumber", entity.PlannedStopNumber, DbType.Int32)
                .AddParameter("RouteHeaderCode", entity.RouteHeaderCode, DbType.String)
                .AddParameter("RouteHeaderId", entity.RouteHeaderId, DbType.Int32)
                .AddParameter("DropId", entity.DropId, DbType.String)
                .AddParameter("Previously", entity.Previously, DbType.String)
                .AddParameter("LocationId", entity.LocationId, DbType.String)
                .AddParameter("DeliveryDate", entity.DeliveryDate, DbType.DateTime)
                .AddParameter("ShellActionIndicator", entity.ShellActionIndicator, DbType.String)
                .AddParameter("AllowOvers", entity.AllowOvers, DbType.Boolean)
                .AddParameter("CustUnatt", entity.CustUnatt, DbType.Boolean)
                .AddParameter("PHUnatt", entity.PHUnatt, DbType.Boolean)
                .AddParameter("StopStatusCode", entity.StopStatusCode, DbType.String)
                .AddParameter("StopStatusDescription", entity.StopStatusDescription, DbType.String)
                .AddParameter("PerformanceStatusCode", entity.PerformanceStatusCode, DbType.String)
                .AddParameter("PerformanceStatusDescription", entity.PerformanceStatusDescription, DbType.String)
                .AddParameter("Reason", entity.StopByPassReason, DbType.String)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("CreatedDate", entity.DateCreated, DbType.DateTime)
                .AddParameter("UpdatedDate", entity.DateUpdated, DbType.DateTime)
                .AddParameter("ActualPaymentCash", entity.ActualPaymentCash, DbType.Decimal)
                .AddParameter("ActualPaymentCheque", entity.ActualPaymentCheque, DbType.Decimal)
                .AddParameter("ActualPaymentCard", entity.ActualPaymentCard, DbType.Decimal)
                .AddParameter("AccountBalance", entity.AccountBalance, DbType.Decimal).Query<int>().FirstOrDefault();
        }

        protected override void UpdateExisting(Stop entity)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.StopUpdate)
                .AddParameter("Id", entity.Id, DbType.Int32)
                .AddParameter("TransportOrderReference", entity.TransportOrderReference, DbType.String)
                .AddParameter("PlannedStopNumber", entity.PlannedStopNumber, DbType.Int32)
                .AddParameter("RouteHeaderCode", entity.RouteHeaderCode, DbType.String)
                .AddParameter("RouteHeaderId", entity.RouteHeaderId, DbType.Int32)
                .AddParameter("DropId", entity.DropId, DbType.String)
                .AddParameter("Previously", entity.Previously, DbType.String)
                .AddParameter("LocationId", entity.LocationId, DbType.String)
                .AddParameter("DeliveryDate", entity.DeliveryDate, DbType.DateTime)
                .AddParameter("ShellActionIndicator", entity.ShellActionIndicator, DbType.String)
                .AddParameter("AllowOvers", entity.AllowOvers, DbType.Boolean)
                .AddParameter("CustUnatt", entity.CustUnatt, DbType.Boolean)
                .AddParameter("PHUnatt", entity.PHUnatt, DbType.Boolean)
                .AddParameter("StopStatusCode", entity.StopStatusCode, DbType.String)
                .AddParameter("StopStatusDescription", entity.StopStatusDescription, DbType.String)
                .AddParameter("PerformanceStatusCode", entity.PerformanceStatusCode, DbType.String)
                .AddParameter("PerformanceStatusDescription", entity.PerformanceStatusDescription, DbType.String)
                .AddParameter("Reason", entity.StopByPassReason, DbType.String)
                .AddParameter("DateDeleted", entity.DateDeleted, DbType.DateTime)
                .AddParameter("ActualPaymentCash", entity.ActualPaymentCash, DbType.Decimal)
                .AddParameter("ActualPaymentCheque", entity.ActualPaymentCheque, DbType.Decimal)
                .AddParameter("ActualPaymentCard", entity.ActualPaymentCard, DbType.Decimal)
                .AddParameter("AccountBalance", entity.AccountBalance, DbType.Decimal)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                //.AddParameter("Location_Id", entity.LocationId, DbType.Int32)
                .Execute();
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
