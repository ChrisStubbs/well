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

        public Job JobGetByRefDetails(string ref1, string ref2, int stopId)
        {
            var job =
               dapperProxy.WithStoredProcedure(StoredProcedures.JobGetByRefDetails)
                   .AddParameter("Ref1", ref1, DbType.String)
                   .AddParameter("Ref2", ref2, DbType.String)
                   .AddParameter("StopId", stopId, DbType.Int32)
                   .Query<Job>()
                   .FirstOrDefault();

            return job;
        }

        public void JobCreateOrUpdate(Job job)
        {
            var jobPerformanceStatusId = job.PerformanceStatusId == 0 ? (int)PerformanceStatus.Notdef : job.PerformanceStatusId;
            var jobByPassReasonId = job.ByPassReasonId == 0 ? (int)ByPassReasons.Notdef : job.ByPassReasonId;

            job.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.JobCreateOrUpdate)
                .AddParameter("Id", job.Id, DbType.Int32)
                .AddParameter("Sequence", job.Sequence, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String)
                .AddParameter("JobTypeCode", job.JobTypeCode, DbType.String)
                .AddParameter("JobRef1", job.JobRef1, DbType.String)
                .AddParameter("JobRef2", job.JobRef2, DbType.String)
                .AddParameter("JobRef3", job.JobRef3, DbType.String)
                .AddParameter("JobRef4", job.JobRef4, DbType.String)
                .AddParameter("OrderDate", job.OrderDate, DbType.DateTime)
                .AddParameter("Originator", job.Originator, DbType.String)
                .AddParameter("TextField1", job.TextField1, DbType.String)
                .AddParameter("TextField2", job.TextField2, DbType.String)
                .AddParameter("PerformanceStatusId", jobPerformanceStatusId, DbType.Int16)
                .AddParameter("ByPassReasonId  ", jobByPassReasonId, DbType.Int16)
                .AddParameter("StopId", job.StopId, DbType.Int32).Query<int>().FirstOrDefault();
        }

        public void AddJobAttributes(Attribute attribute)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.JobAttributeCreateOrUpdate)
                .AddParameter("Id", attribute.Id, DbType.Int32)
                .AddParameter("Code", attribute.Code, DbType.String)
                .AddParameter("Value", attribute.Value1, DbType.String)
                .AddParameter("JobId", attribute.AttributeId, DbType.Int32)
                .AddParameter("Username", this.CurrentUser, DbType.String).Query<int>();
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

        public void DeleteJobById(int id, WellDeleteType deleteType)
        {
            var isSoftDelete = deleteType == WellDeleteType.SoftDelete;

            DeleteJobAttributesJobById(id, isSoftDelete);

            dapperProxy.WithStoredProcedure(StoredProcedures.JobDeleteById)
                .AddParameter("JobId", id, DbType.Int32)
                .AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean)
                .Execute();
        }

        private void DeleteJobAttributesJobById(int id, bool isSoftDelete)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.JobArttributesDeleteById)
                .AddParameter("JobId", id, DbType.Int32)
                .AddParameter("IsSoftDelete", isSoftDelete, DbType.Boolean)
                .Execute();
        }

        public IEnumerable<PodActionReasons> GetPodActionReasonsById(int pdaCreditReasonId)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.JobGetCreditActionReasons)
                   .AddParameter("PDACreditReasonId", pdaCreditReasonId, DbType.Int32)
                   .Query<PodActionReasons>();
        }




    }
}
