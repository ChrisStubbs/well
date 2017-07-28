namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Dapper;
    using Domain.ValueObjects;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    public class BranchRepository : DapperRepository<Branch, int>, IBranchRepository
    {
        public BranchRepository(ILogger logger, IDapperProxy dapperProxy, IUserNameProvider userNameProvider)
            : base(logger, dapperProxy, userNameProvider)
        {
        }

        public IEnumerable<Branch> GetAll()
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.BranchesGet).QueryMultiples(GetFromGrid);
        }

        public IEnumerable<Branch> GetAllValidBranches()
        {
            var branches = GetAll();
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

        public int GetBranchIdForJob(int jobId)
        {
            return
                this.dapperProxy.WithStoredProcedure(StoredProcedures.GetBranchIdForJob)
                    .AddParameter("jobId", jobId, DbType.Int32)
                    .Query<int>()
                    .Single();
        }

        public int GetBranchIdForStop(int stopId)
        {
            return
                this.dapperProxy.WithStoredProcedure(StoredProcedures.GetBranchIdForStop)
                    .AddParameter("stopId", stopId, DbType.Int32)
                    .Query<int>()
                    .Single();
        }


        private IEnumerable<Branch> GetFromGrid(SqlMapper.GridReader grid)
        {
            var branches = grid.Read<Branch>().ToList();
            var creditThresholds = grid.Read<BranchCreditThreshold>().ToList();

            foreach (var branch in branches)
            {
                branch.CreditThresholds = creditThresholds.Where(c => c.BranchId == branch.Id).ToList();
            }

            return branches;
        }
    }
}