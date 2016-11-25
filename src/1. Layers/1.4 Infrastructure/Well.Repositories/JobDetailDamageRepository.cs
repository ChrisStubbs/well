namespace PH.Well.Repositories
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;

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
            var damageSource = entity.JobDetailDamageSource == null
                ? (int)JobDetailDamageSource.NotDef
                : (int)entity.JobDetailDamageSource;

            entity.Id = dapperProxy.WithStoredProcedure("JobDetailDamage_Insert")
                .AddParameter("JobDetailId", entity.JobDetailId, DbType.Int32)
                .AddParameter("Qty", entity.Qty, DbType.Decimal)
                .AddParameter("DamageReasonsId", (int) entity.DamageReason, DbType.Int16)
                .AddParameter("DamageSourceId", damageSource, DbType.Int16)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("DateCreated", entity.DateCreated, DbType.DateTime)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                .Query<int>().FirstOrDefault();
        }

        protected override void UpdateExisting (JobDetailDamage entity)
        {
            var damageSource = entity.JobDetailDamageSource == null
                ? (int) JobDetailDamageSource.NotDef
                : (int) entity.JobDetailDamageSource;

            dapperProxy.WithStoredProcedure("JobDetailDamage_Update")
                .AddParameter("Id", entity.Id, DbType.Int32)
                .AddParameter("JobDetailId", entity.JobDetailId, DbType.Int32)
                .AddParameter("Qty", entity.Qty, DbType.Decimal)
                .AddParameter("DamageReasonsId", (int) entity.DamageReason, DbType.Int16)
                .AddParameter("DamageSourceId", damageSource, DbType.Int16)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                .Execute();
        }

        public void Delete(int jobDetailId)
        {
            dapperProxy.WithStoredProcedure("JobDetailDamage_Delete")
                .AddParameter("JobDetailId", jobDetailId, DbType.Int32)
                .Execute();
        }

    }
}
