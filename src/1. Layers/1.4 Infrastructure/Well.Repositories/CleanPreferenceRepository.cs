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

    public class CleanPreferenceRepository : DapperRepository<CleanPreference, int>, ICleanPreferenceRepository
    {
        public CleanPreferenceRepository(ILogger logger, IDapperProxy dapperProxy)
            : base(logger, dapperProxy)
        {
        }

        public IEnumerable<CleanPreference> GetAll()
        {
            var cleanPreferences = this.dapperProxy.WithStoredProcedure(StoredProcedures.CleanPreferencesGetAll).Query<CleanPreference>();

            foreach (var cleanPreference in cleanPreferences)
            {
                var branches = this.dapperProxy.WithStoredProcedure(StoredProcedures.CleanPreferencesBranchesGet)
                    .AddParameter("cleanPreferenceId", cleanPreference.Id, DbType.Int32).Query<Branch>();

                branches.ForEach(x => cleanPreference.Branches.Add(x));
            }

            return cleanPreferences;
        }

        public CleanPreference GetByBranchId(int branchId)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.CleanPreferenceByBranchGet)
                .AddParameter("branchId", branchId, DbType.Int32)
                .Query<CleanPreference>()
                .FirstOrDefault();
        }

        protected override void SaveNew(CleanPreference entity)
        {
            using (
                var transactionScope = new TransactionScope(
                    TransactionScopeOption.Required,
                    TimeSpan.FromMinutes(Configuration.TransactionTimeout)))
            {
                if (!entity.IsTransient()) this.Delete(entity.Id);

                entity.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.CleanPreferenceSave)
                    .AddParameter("Days", entity.Days, DbType.Int32)
                    .AddParameter("DateCreated", entity.DateCreated, DbType.DateTime)
                    .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                    .AddParameter("CreatedBy", entity.CreatedBy, DbType.String, size: 50)
                    .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String, size: 50)
                    .Query<int>().Single();

                foreach (var branch in entity.Branches)
                {
                    this.dapperProxy.WithStoredProcedure(StoredProcedures.CleanPreferenceToBranchSave)
                        .AddParameter("BranchId", branch.Id, DbType.Int32)
                        .AddParameter("CleanPreferenceId", entity.Id, DbType.Int32)
                        .Execute();
                }

                transactionScope.Complete();
            }
        }

        public void Delete(int id)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.CleanPreferenceDelete)
                .AddParameter("Id", id, DbType.Int32).Execute();
        }
    }
}