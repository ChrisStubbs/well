namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;
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

        public void JobCreateOrUpdate(Job job)
        {

            var credit = job.TotalCreditValueForThreshold();

            job.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.JobCreateOrUpdate)
                .AddParameter("Id", job.Id, DbType.Int32)
                .AddParameter("Sequence", job.Sequence, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String)
                .AddParameter("JobTypeCode", job.JobTypeCode, DbType.String)
                .AddParameter("PHAccount", job.PhAccount, DbType.String)
                .AddParameter("PickListRef", job.PickListRef, DbType.String)
                .AddParameter("InvoiceNumber", string.IsNullOrWhiteSpace(job.InvoiceNumber) ? null : job.InvoiceNumber, DbType.String)
                .AddParameter("CustomerRef", job.CustomerRef, DbType.String)
                .AddParameter("OrderDate", job.OrderDate, DbType.DateTime)
                .AddParameter("RoyaltyCode", job.RoyaltyCode, DbType.String)
                .AddParameter("RoyaltyCodeDesc", job.RoyaltyCodeDesc, DbType.String)
                .AddParameter("OrdOuters", job.OrdOuters, DbType.Int16)
                .AddParameter("InvOuters", job.InvOuters, DbType.Int16)
                .AddParameter("ColOuters", job.ColOuters, DbType.Int16)
                .AddParameter("ColBoxes", job.ColBoxes, DbType.Int16)
                .AddParameter("ReCallPrd", job.ReCallPrd, DbType.Boolean)
                .AddParameter("AllowSgCrd", job.AllowSgCrd, DbType.Boolean)
                .AddParameter("AllowSOCrd", job.AllowSoCrd, DbType.Boolean)
                .AddParameter("COD", job.Cod, DbType.Boolean)
                .AddParameter("GrnNumber", job.GrnNumber, DbType.String)
                .AddParameter("GrnRefusedReason", job.GrnRefusedReason, DbType.String)
                .AddParameter("GrnRefusedDesc", job.GrnRefusedDesc, DbType.String)
                .AddParameter("AllowReOrd", job.AllowReOrd, DbType.Boolean)
                .AddParameter("SandwchOrd", job.SandwchOrd, DbType.Boolean)
                .AddParameter("ComdtyType", job.ComdtyType, DbType.String)
                .AddParameter("TotalCreditValueForThreshold", job.TotalCreditValueForThreshold(), DbType.Decimal)
                .AddParameter("PerformanceStatusId", (int)job.PerformanceStatus, DbType.Int16)
                .AddParameter("ByPassReasonId  ", (int)job.ByPassReason, DbType.Int16)
                .AddParameter("StopId", job.StopId, DbType.Int32).Query<int>().FirstOrDefault();
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

        public IEnumerable<PodActionReasons> GetPodActionReasonsById(int pdaCreditReasonId)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.JobGetCreditActionReasons)
                   .AddParameter("PDACreditReasonId", pdaCreditReasonId, DbType.Int32)
                   .Query<PodActionReasons>();
        }
        public void CreditLines(DataTable idsTable)
        {
            dapperProxy.WithStoredProcedure("Job_CreditLines")
                .AddParameter("Ids", idsTable, DbType.Object)
                .Execute();
        }


       
    }
}
