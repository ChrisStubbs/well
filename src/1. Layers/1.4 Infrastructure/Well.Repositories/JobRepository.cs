namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;

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

        public Job JobCreateOrUpdate(Job job)
        {
            var id = this.dapperProxy.WithStoredProcedure(StoredProcedures.JobCreateOrUpdate)
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
                .AddParameter("PerformanceStatusCode", job.PerformanceStatusCode, DbType.String)
                .AddParameter("StopId", job.StopId, DbType.Int32).Query<int>().FirstOrDefault();

            return this.GetById(id);

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

    }
}
