namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;

    public class JobDetailDamageRepository : DapperRepository<JobDetailDamage, int>, IJobDetailDamageRepository
    {
        public JobDetailDamageRepository(ILogger logger, IWellDapperProxy dapperProxy) : base(logger, dapperProxy)
        {
        }

        public IEnumerable<JobDetailDamage> GetJobDamagesByJobDetailId(int jobDetailId)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailDamageGetByJobDetailId)
                .AddParameter("JobDetailId", jobDetailId, DbType.Int32)
                .Query<JobDetailDamage>().ToList();
        }

        protected override void SaveNew(JobDetailDamage entity)
        {
            entity.Id = dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailDamageInsert)
                .AddParameter("JobDetailId", entity.JobDetailId, DbType.Int32)
                .AddParameter("JobDetailSourceId", entity.JobDetailSourceId, DbType.Int16)
                .AddParameter("JobDetailReasonId", entity.JobDetailReasonId, DbType.Int16)
                .AddParameter("DamageActionId", entity.DamageActionId, DbType.Int32)
                .AddParameter("Qty", entity.Qty, DbType.Int32)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("DateCreated", entity.DateCreated, DbType.DateTime)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                .Query<int>().FirstOrDefault();
        }

        protected override void UpdateExisting (JobDetailDamage entity)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailDamageUpdate)
                .AddParameter("Id", entity.Id, DbType.Int32)
                .AddParameter("JobDetailId", entity.JobDetailId, DbType.Int32)
                .AddParameter("Qty", entity.Qty, DbType.Int32)
                .AddParameter("JobDetailSourceId", entity.JobDetailSource, DbType.Int16)
                .AddParameter("JobDetailReasonId", entity.JobDetailReason, DbType.Int16)
                .AddParameter("DamageActionId", entity.DamageActionId, DbType.Int16)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                .Execute();
        }

        public void Delete(int jobDetailId)
        {
            dapperProxy.WithStoredProcedure(StoredProcedures.JobDetailDamageDelete)
                .AddParameter("JobDetailId", jobDetailId, DbType.Int32)
                .Execute();
        }

    }
}
