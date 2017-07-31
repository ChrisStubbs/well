namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Transactions;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    using WebGrease.Css.Extensions;

    public class CreditThresholdRepository : DapperRepository<CreditThreshold, int>, ICreditThresholdRepository
    {
        public CreditThresholdRepository(ILogger logger, IDapperProxy dapperProxy, IUserNameProvider userNameProvider)
            : base(logger, dapperProxy, userNameProvider)
        {
        }

        protected override void SaveNew(CreditThreshold entity)
        {
            entity.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdSave)
                .AddParameter("Level", (int) entity.Level, DbType.Int32)
                .AddParameter("Value", entity.Threshold, DbType.Decimal)
                .AddParameter("DateCreated", entity.DateCreated, DbType.DateTime)
                .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String, size: 50)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String, size: 50)
                .Query<int>().Single();
        }

        public IEnumerable<CreditThreshold> GetAll()
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdGetAll).Query<CreditThreshold>();
        }

        public void Delete(int id)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdDelete)
                .AddParameter("Id", id, DbType.Int32).Execute();
        }

        public CreditThreshold GetById(int thresholdId)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdGetAll).Query<CreditThreshold>()
                .FirstOrDefault(x => x.Id == thresholdId);
        }

        public void PendingCreditInsert(int jobId)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.PendingCreditInsert)
                .AddParameter("jobId", jobId, DbType.Int32)
                .AddParameter("originator", this.CurrentUser, DbType.String)
                .Execute();
        }
    }
}
