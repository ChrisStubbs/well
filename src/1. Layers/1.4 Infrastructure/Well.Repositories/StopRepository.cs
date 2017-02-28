namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;

    public class StopRepository : DapperRepository<Stop, int>, IStopRepository
    {
        public StopRepository(ILogger logger, IWellDapperProxy dapperProxy, IUserNameProvider userNameProvider) 
            : base(logger, dapperProxy, userNameProvider)
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
                .AddParameter("AllowOvers", entity.AllowOvers == "True", DbType.Boolean)
                .AddParameter("CustUnatt", entity.CustUnatt == "True", DbType.Boolean)
                .AddParameter("PHUnatt", entity.PHUnatt == "True", DbType.Boolean)
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
                .AddParameter("StopStatusCode", entity.StopStatusCode, DbType.String)
                .AddParameter("StopStatusDescription", entity.StopStatusDescription, DbType.String)
                .AddParameter("PerformanceStatusCode", entity.PerformanceStatusCode, DbType.String)
                .AddParameter("PerformanceStatusDescription", entity.PerformanceStatusDescription, DbType.String)
                .AddParameter("Reason", entity.StopByPassReason, DbType.String)
                .AddParameter("ShellActionIndicator", entity.ShellActionIndicator, DbType.String)
                .AddParameter("ActualPaymentCash", entity.ActualPaymentCash, DbType.Decimal)
                .AddParameter("ActualPaymentCheque", entity.ActualPaymentCheque, DbType.Decimal)
                .AddParameter("ActualPaymentCard", entity.ActualPaymentCard, DbType.Decimal)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("UpdatedDate", entity.DateUpdated, DbType.DateTime).Execute();
        }

        public Stop GetByJobDetails(string picklist, string account)
        {
            return
               dapperProxy.WithStoredProcedure(StoredProcedures.StopGetByJob)
                   .AddParameter("Picklist", picklist, DbType.String)
                   .AddParameter("Account", account, DbType.String)
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
