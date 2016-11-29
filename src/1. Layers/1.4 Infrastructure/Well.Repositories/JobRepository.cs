namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.ValueObjects;

    public class JobRepository : DapperRepository<Job, int>, IJobRepository
    {
        public JobRepository(ILogger logger, IWellDapperProxy dapperProxy) : base(logger, dapperProxy)
        {
        }

        public Job GetById(int id)
        {
            var job =
               dapperProxy.WithStoredProcedure(StoredProcedures.JobGetById)
                   .AddParameter("Id", id, DbType.Int32)
                   .Query<Job>()
                   .FirstOrDefault();

            return job;
        }

        public IEnumerable<Job> GetByStopId(int id)
        {
             return    dapperProxy.WithStoredProcedure(StoredProcedures.JobGetByStopId)
                    .AddParameter("StopId", id, DbType.Int32)
                    .Query<Job>();
        }

        public IEnumerable<CustomerRoyaltyException>  GetCustomerRoyaltyExceptions()
        {
            var customerRoyaltyException =
                dapperProxy.WithStoredProcedure(StoredProcedures.CustomerRoyalExceptionGet)
                    .Query<CustomerRoyaltyException>();

            return customerRoyaltyException;
        }

        public CustomerRoyaltyException GetCustomerRoyaltyExceptionsByRoyalty(int royalty)
        {
            var customerRoyaltyException =
                dapperProxy.WithStoredProcedure(StoredProcedures.CustomerRoyalExceptionGetByRoyalty)
                .AddParameter("RoyaltyId", royalty, DbType.String)
                    .Query<CustomerRoyaltyException>();

            return customerRoyaltyException.FirstOrDefault();
        }
        
        public void AddCustomerRoyaltyException(CustomerRoyaltyException royaltyException)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.CustomerRoyaltyExceptionInsert)
               .AddParameter("RoyaltyId", royaltyException.RoyaltyId, DbType.String)
               .AddParameter("Customer", royaltyException.Customer, DbType.String)
               .AddParameter("ExceptionDays", royaltyException.ExceptionDays, DbType.Int32)
               .AddParameter("Username", this.CurrentUser, DbType.String).Query<int>();
        }

        public void UpdateCustomerRoyaltyException(CustomerRoyaltyException royaltyException)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.CustomerRoyaltyExceptionUpdate)
                .AddParameter("Id", royaltyException.Id, DbType.Int32)
               .AddParameter("RoyaltyId", royaltyException.RoyaltyId, DbType.String)
               .AddParameter("Customer", royaltyException.Customer, DbType.String)
               .AddParameter("ExceptionDays", royaltyException.ExceptionDays, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String).Query<int>();
        }

        public Job JobGetByRefDetails(string phAccount, string pickListRef, int stopId)
        {
            var job =
               dapperProxy.WithStoredProcedure(StoredProcedures.JobGetByRefDetails)
                   .AddParameter("PHAccount", phAccount, DbType.String)
                   .AddParameter("PickListRef", pickListRef, DbType.String)
                   .AddParameter("StopId", stopId, DbType.Int32)
                   .Query<Job>()
                   .FirstOrDefault();

            return job;
        }

        protected override void SaveNew(Job entity)
        {
            entity.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.JobInsert)
                .AddParameter("Sequence", entity.Sequence, DbType.Int32)
                .AddParameter("JobTypeCode", entity.JobTypeCode, DbType.String)
                .AddParameter("PHAccount", entity.PhAccount, DbType.String)
                .AddParameter("PickListRef", entity.PickListRef, DbType.String)
                .AddParameter("InvoiceNumber", entity.InvoiceNumber, DbType.String)
                .AddParameter("CustomerRef", entity.CustomerRef, DbType.String)
                .AddParameter("OrderDate", entity.OrderDate, DbType.DateTime)
                .AddParameter("RoyaltyCode", entity.RoyaltyCode, DbType.String)
                .AddParameter("RoyaltyCodeDesc", entity.RoyaltyCodeDesc, DbType.String)
                .AddParameter("OrdOuters", entity.OrdOuters, DbType.Int16)
                .AddParameter("InvOuters", entity.InvOuters, DbType.Int16)
                .AddParameter("ColOuters", entity.ColOuters, DbType.Int16)
                .AddParameter("ColBoxes", entity.ColBoxes, DbType.Int16)
                .AddParameter("ReCallPrd", entity.ReCallPrd, DbType.Boolean)
                .AddParameter("AllowSOCrd", entity.AllowSoCrd, DbType.Boolean)
                .AddParameter("COD", entity.Cod, DbType.Boolean)
                .AddParameter("GrnNumber", entity.GrnNumber, DbType.String)
                .AddParameter("GrnRefusedReason", entity.GrnRefusedReason, DbType.String)
                .AddParameter("GrnRefusedDesc", entity.GrnRefusedDesc, DbType.String)
                .AddParameter("AllowReOrd", entity.AllowReOrd, DbType.Boolean)
                .AddParameter("SandwchOrd", entity.SandwchOrd, DbType.Boolean)
                .AddParameter("TotalCreditValueForThreshold", entity.TotalCreditValueForThreshold(), DbType.Decimal)
                .AddParameter("PerformanceStatusId", (int)entity.PerformanceStatus, DbType.Int16)
                .AddParameter("ByPassReasonId  ", (int)entity.ByPassReason, DbType.Int16)
                .AddParameter("StopId", entity.StopId, DbType.Int32)
                .AddParameter("ActionLogNumber", entity.ActionLogNumber, DbType.String)
                .AddParameter("OuterCount", entity.OuterCount, DbType.Int16)
                .AddParameter("OuterDiscrepancyFound", entity.OuterDiscrepancyFound, DbType.String)
                .AddParameter("TotalOutersOver", entity.TotalOutersOver, DbType.Int16)
                .AddParameter("TotalOutersShort", entity.TotalOutersShort, DbType.Int16)
                .AddParameter("Picked", entity.Picked, DbType.Boolean)
                .AddParameter("InvoiceValue", entity.InvoiceValue, DbType.Decimal)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("CreatedDate", entity.DateCreated, DbType.DateTime)
                .AddParameter("UpdatedDate", entity.DateUpdated, DbType.DateTime)
                .Query<int>().FirstOrDefault();
        }

        public void JobCreateOrUpdate(Job job)
        {
            job.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.JobCreateOrUpdate)
                .AddParameter("Id", job.Id, DbType.Int32)
                .AddParameter("Sequence", job.Sequence, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String)
                .AddParameter("JobTypeCode", job.JobTypeCode, DbType.String)
                .AddParameter("PHAccount", job.PhAccount, DbType.String)
                .AddParameter("PickListRef", job.PickListRef, DbType.String)
                .AddParameter("InvoiceNumber", job.InvoiceNumber, DbType.String)
                .AddParameter("CustomerRef", job.CustomerRef, DbType.String)
                .AddParameter("OrderDate", job.OrderDate, DbType.DateTime)
                .AddParameter("RoyaltyCode", job.RoyaltyCode, DbType.String)
                .AddParameter("RoyaltyCodeDesc", job.RoyaltyCodeDesc, DbType.String)
                .AddParameter("OrdOuters", job.OrdOuters, DbType.Int16)
                .AddParameter("InvOuters", job.InvOuters, DbType.Int16)
                .AddParameter("ColOuters", job.ColOuters, DbType.Int16)
                .AddParameter("ColBoxes", job.ColBoxes, DbType.Int16)
                .AddParameter("ReCallPrd", job.ReCallPrd, DbType.Boolean)
                .AddParameter("AllowSOCrd", job.AllowSoCrd, DbType.Boolean)
                .AddParameter("COD", job.Cod, DbType.Boolean)
                .AddParameter("GrnNumber", job.GrnNumber, DbType.String)
                .AddParameter("GrnRefusedReason", job.GrnRefusedReason, DbType.String)
                .AddParameter("GrnRefusedDesc", job.GrnRefusedDesc, DbType.String)
                .AddParameter("AllowReOrd", job.AllowReOrd, DbType.Boolean)
                .AddParameter("SandwchOrd", job.SandwchOrd, DbType.Boolean)
                .AddParameter("TotalCreditValueForThreshold", job.TotalCreditValueForThreshold(), DbType.Decimal)
                .AddParameter("PerformanceStatusId", (int) job.PerformanceStatus, DbType.Int16)
                .AddParameter("ByPassReasonId  ", (int) job.ByPassReason, DbType.Int16)
                .AddParameter("StopId", job.StopId, DbType.Int32)
                .AddParameter("ActionLogNumber", job.ActionLogNumber, DbType.String)
                .AddParameter("OuterCount", job.OuterCount, DbType.Int16)
                .AddParameter("OuterDiscrepancyFound", job.OuterDiscrepancyFound, DbType.String)
                .AddParameter("TotalOutersOver", job.TotalOutersOver, DbType.Int16)
                .AddParameter("TotalOutersShort", job.TotalOutersShort, DbType.Int16)
                .AddParameter("Picked", job.Picked, DbType.Boolean)
                .AddParameter("InvoiceValue", job.InvoiceValue, DbType.Decimal)
                .Query<int>().FirstOrDefault()
                ;

        }
        
        public Job GetByAccountPicklistAndStopId(string accountId, string picklistId, int stopId)
        {
            var job =
               dapperProxy.WithStoredProcedure(StoredProcedures.JobGetByAccountPicklistAndStopId)
                .AddParameter("AccountId", accountId, DbType.String)
                .AddParameter("PicklistId", picklistId, DbType.String)
                .AddParameter("StopId", stopId, DbType.Int32)
                .Query<Job>()
                .FirstOrDefault();

            return job;
        }

        public void DeleteJobById(int id)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.JobDeleteById)
                .AddParameter("JobId", id, DbType.Int32)
                .Execute();
        }

        protected override void UpdateExisting(Job entity)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.JobUpdate)
                .AddParameter("Id", entity.Id, DbType.Int32)
                .AddParameter("ByPassReason", (int)entity.ByPassReason, DbType.Int16)
                .AddParameter("PerformanceStatus", (int)entity.PerformanceStatus, DbType.Int16)
                .AddParameter("InvoiceNumber", entity.InvoiceNumber, DbType.String)
                .AddParameter("Sequence", entity.Sequence, DbType.Int32)
                .AddParameter("JobTypeCode", entity.JobTypeCode, DbType.String)
                .AddParameter("PhAccount", entity.PhAccount, DbType.String)
                .AddParameter("CustomerRef", entity.CustomerRef, DbType.String)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("UpdatedDate", entity.DateUpdated, DbType.DateTime).Execute();
        }

        public IEnumerable<PodActionReasons> GetPodActionReasonsById(int pdaCreditReasonId)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.JobGetCreditActionReasons)
                   .AddParameter("PDACreditReasonId", pdaCreditReasonId, DbType.Int32)
                   .Query<PodActionReasons>();
        }

        public void CreditLines(DataTable creditLinesTable)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.CreditJob)
                .AddParameter("CreditLines", creditLinesTable, DbType.Object)
                .Execute();
        }

        public void JobPendingCredits(DataTable creditLinesTable, int userId)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.PendingCreditsInsert)
                .AddParameter("CreditLines", creditLinesTable, DbType.Object)
                .AddParameter("UserId", userId, DbType.Int32)
                .Execute();
        }

        public void ResolveJobAndJobDetails(int jobId)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.ResolveJobAndJobDetails)
                .AddParameter("jobId", jobId, DbType.Int32)
                .Execute();                
        }
    }
}
