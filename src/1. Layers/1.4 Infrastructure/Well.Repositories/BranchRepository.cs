namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    public class BranchRepository : DapperRepository<Branch, int>, IBranchRepository
    {
        public BranchRepository(ILogger logger, IDapperProxy dapperProxy)
            : base(logger, dapperProxy)
        {
        }

        public IEnumerable<Branch> GetAll()
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.BranchesGet).Query<Branch>();
        }

        public IEnumerable<Branch> GetAllValidBranches()
        {
            var branches = this.dapperProxy.WithStoredProcedure(StoredProcedures.BranchesGet).Query<Branch>().ToList();

            return branches.Where(x => x.Id != (int)Domain.Enums.Branch.NotDefined);
        }

        public void DeleteUserBranches(User user)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.DeleteUserBranches)
                .AddParameter("UserId", user.Id, DbType.Int32).Execute();
        }

        public void SaveBranchesForUser(IEnumerable<Branch> branches, User user)
        {
            foreach (var branch in branches)
            {
                this.dapperProxy.WithStoredProcedure(StoredProcedures.SaveUserBranch)
                    .AddParameter("UserId", user.Id, DbType.Int32)
                    .AddParameter("BranchId", branch.Id, DbType.Int32)
                    .AddParameter("CreatedBy", this.CurrentUser, DbType.String, size: 50)
                    .AddParameter("DateCreated", DateTime.Now, DbType.DateTime)
                    .AddParameter("UpdatedBy", this.CurrentUser, DbType.String, size: 50)
                    .AddParameter("DateUpdated", DateTime.Now, DbType.DateTime)
                    .Execute();
            }
        }

        public IEnumerable<Branch> GetBranchesForUser(string username)
        {
            return
                this.dapperProxy.WithStoredProcedure(StoredProcedures.GetBranchesForUser)
                    .AddParameter("Name", username, DbType.String, size: 255)
                    .Query<Branch>();
        }

        public IEnumerable<Branch> GetBranchesForSeasonalDate(int seasonalDateId)
        {
            return
                this.dapperProxy.WithStoredProcedure(StoredProcedures.GetBranchesForSeasonalDate)
                    .AddParameter("SeasonalDateId", seasonalDateId, DbType.Int32)
                    .Query<Branch>();
        }

        public IEnumerable<Branch> GetBranchesForCreditThreshold(int creditThresholdId)
        {
            return
                this.dapperProxy.WithStoredProcedure(StoredProcedures.GetBranchesForCreditThreshold)
                    .AddParameter("CreditThresholdId", creditThresholdId, DbType.Int32)
                    .Query<Branch>();
        }

        public IEnumerable<Branch> GetBranchesForCleanPreference(int cleanPreferenceId)
        {
            return
                this.dapperProxy.WithStoredProcedure(StoredProcedures.GetBranchesForCleanPreference)
                    .AddParameter("CleanPreferenceId", cleanPreferenceId, DbType.Int32)
                    .Query<Branch>();
        }
    }
}