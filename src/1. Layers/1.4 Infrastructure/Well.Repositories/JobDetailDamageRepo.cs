﻿namespace PH.Well.Repositories
{
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;

    public class JobDetailDamageRepo : DapperRepository<JobDetailDamage, int>, IJobDetailDamageRepo
    {
        public JobDetailDamageRepo(ILogger logger, IWellDapperProxy dapperProxy) : base(logger, dapperProxy)
        {
        }

        protected override void SaveNew(JobDetailDamage entity)
        {
            entity.Id = dapperProxy.WithStoredProcedure("JobDetailDamage_Insert")
                .AddParameter("JobDetailId", entity.JobDetailId, DbType.Int32)
                .AddParameter("Qty", entity.Qty, DbType.Decimal)
                .AddParameter("DamageReasonsId", (int) entity.DamageReason, DbType.Int16)
                .AddParameter("DamageSourceId", (int)entity.JobDetailDamageSource, DbType.Int16)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("DateCreated", entity.DateCreated, DbType.DateTime)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                .Query<int>().FirstOrDefault();
        }

        protected override void UpdateExisting (JobDetailDamage entity)
        {
            dapperProxy.WithStoredProcedure("JobDetailDamage_Update")
                .AddParameter("Id", entity.Id, DbType.Int32)
                .AddParameter("JobDetailId", entity.JobDetailId, DbType.Int32)
                .AddParameter("Qty", entity.Qty, DbType.Decimal)
                .AddParameter("DamageReasonsId", (int) entity.DamageReason, DbType.Int16)
                .AddParameter("DamageSourceId", (int)entity.JobDetailDamageSource, DbType.Int16)
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
