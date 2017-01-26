namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Transactions;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;

    using WebGrease.Css.Extensions;

    public class CreditThresholdRepository : DapperRepository<CreditThreshold, int>, ICreditThresholdRepository
    {
        public CreditThresholdRepository(ILogger logger, IDapperProxy dapperProxy)
            : base(logger, dapperProxy)
        {
        }

        protected override void SaveNew(CreditThreshold entity)
        {
            using (
                var transactionScope = new TransactionScope(
                    TransactionScopeOption.Required,
                    TimeSpan.FromMinutes(Configuration.TransactionTimeout)))
            {
                if (!entity.IsTransient()) this.Delete(entity.Id);

                entity.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdSave)
                    .AddParameter("ThresholdLevelId", entity.ThresholdLevelId, DbType.Int32)
                    .AddParameter("Threshold", entity.Threshold, DbType.Int32)
                    .AddParameter("DateCreated", entity.DateCreated, DbType.DateTime)
                    .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                    .AddParameter("CreatedBy", entity.CreatedBy, DbType.String, size: 50)
                    .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String, size: 50)
                    .Query<int>().Single();

                foreach (var branch in entity.Branches)
                {
                    this.dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdToBranchSave)
                        .AddParameter("BranchId", branch.Id, DbType.Int32)
                        .AddParameter("CreditThresholdId", entity.Id, DbType.Int32)
                        .Execute();
                }

                transactionScope.Complete();
            }
        }

        public IEnumerable<CreditThreshold> GetAll()
        {
            var thresholds = this.dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdGetAll).Query<CreditThreshold>();

            foreach (var threshold in thresholds)
            {
                var branches = this.dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdBranchesGet)
                    .AddParameter("creditThresholdId", threshold.Id, DbType.Int32).Query<Branch>();

                branches.ForEach(x => threshold.Branches.Add(x));
            }

            return thresholds;
        }

        public void Delete(int id)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdDelete)
                .AddParameter("Id", id, DbType.Int32).Execute();
        }

        public IEnumerable<CreditThreshold> GetByBranch(int branchId)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdByBranch)
                .AddParameter("branchId", branchId, DbType.Int32)
                .Query<CreditThreshold>();
        }

        public void AssignPendingCreditToUser(User user, int jobId, string originator)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.AssignPendingCreditToUser)
                .AddParameter("userId", user.Id, DbType.Int32)
                .AddParameter("jobId", jobId, DbType.Int32)
                .AddParameter("originator", originator, DbType.String)
                .Execute();
        }
    }
}