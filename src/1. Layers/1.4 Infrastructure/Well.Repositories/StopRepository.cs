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

        public IList<int> GetStopByTransportOrderRefIncludingSoftDeleted(IList<string> transportOrderReferences)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.StopIdsGetByTransportOrderReference)
                .AddParameter("transportOrderReferences", transportOrderReferences.ToSingleColumnDataTable("transportOrderRef"), DbType.Object)
                .Query<int>().ToList();

        }

        public Stop GetByJobDetails(string picklist, string account, int branchId)
        {
            var stopIds =
                dapperProxy.WithStoredProcedure(StoredProcedures.StopGetByJob)
                    .AddParameter("Picklist", picklist, DbType.String)
                    .AddParameter("Account", account, DbType.String)
                    .AddParameter("BranchId", branchId, DbType.Int32)
                    .Query<int>();

            return GetByIds(stopIds).FirstOrDefault();
        }

        public IList<Stop> GetByIds(IEnumerable<int> stopIds)
        {
            IList<Stop> stops = new List<Stop>();

            this.dapperProxy.WithStoredProcedure(StoredProcedures.StopsGetByIds)
                .AddParameter("Ids", stopIds.Distinct().ToList().ToIntDataTables("Ids"), DbType.Object)
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
                .AddParameter("DeletedByImport", entity.DeletedByImport, DbType.Boolean)
                .AddParameter("WellStatus", (int) entity.WellStatus, DbType.Int32)
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
                .AddParameter("UpdatedBy", CurrentUser, DbType.String)
                .Execute();
        }

        private void AccountDeleteByStopId(int stopId)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.AccountDeleteByStopId)
                .AddParameter("StopId", stopId, DbType.Int32)
                .Execute();
        }

        public void ReinstateStopSoftDeletedByImport(IList<int> stopIds)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.StopsReinstateSoftDeletedByImport)
                .AddParameter("StopIds", stopIds.ToIntDataTables("StopIds"), DbType.Object)
                .AddParameter("UpdatedBy", CurrentUser, DbType.String)
                .Execute();
        }
        
        public void UpdateWellStatus(IList<Stop> stop)
        {
            var data = stop
                .Select(p => new
                {
                    EntityKey = p.Id,
                    WellStatus = (int)p.WellStatus
                })
                .ToList();

            this.dapperProxy.WithStoredProcedure(StoredProcedures.StopUpdateWellStatus)
                .AddParameter("Data", data.ToDataTables(), DbType.Object)
                .Execute();
        }

        public Stop GetForWellStatusCalculationById(int stopId)
        {
            var stop = dapperProxy.WithStoredProcedure(StoredProcedures.StopGetForWellStatusCalculationById)
                .AddParameter("Id", stopId, DbType.Int32)
                .Query<Stop>()
                .SingleOrDefault();

            if (stop != null)
            {
                stop.Jobs = dapperProxy.WithStoredProcedure(StoredProcedures.JobGetForWellStatusCalculationByStopId)
                    .AddParameter("StopId", stopId, DbType.Int32)
                    .Query<Job>().ToList();
            }

            return stop;
        }

        public IList<Stop> GetForWellStatusCalculationById(IList<int> stopId)
        {
            //i am refactoring this queries 
            //the idea is to get all jobs from all stops in a single query calculate the well status
            //and save
            //classes:
            //    ImportService.UpdateWellStatusForStops =>
            //    StopService.ComputeWellStatus(IList<int> stopId) =>
            //    get the data calculate and save

            return dapperProxy.WithStoredProcedure(StoredProcedures.Job_GetForWellStatusCalculationByStopIds)
                    .AddParameter("StopIds", stopId.ToIntDataTables("StopIds"), DbType.Object)
                    .Query<Job>()
                    .GroupBy(p => p.StopId)
                    .Select(p =>
                    {
                        return new Stop
                        {
                            Id = p.Key,
                            Jobs = p.ToList()
                        };
                    })
                    .ToList();
        }
    }
}
