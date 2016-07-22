namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;

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
                    .AddParameter("CreatedBy", user.Name, DbType.String, size: 50)
                    .AddParameter("DateCreated", DateTime.Now, DbType.DateTime)
                    .AddParameter("UpdatedBy", user.Name, DbType.String, size: 50)
                    .AddParameter("DateUpdated", DateTime.Now, DbType.DateTime)
                    .Execute();
            }
        }

        public IEnumerable<Branch> GetBranchesForUser(string username)
        {
            return
                this.dapperProxy.WithStoredProcedure(StoredProcedures.GetBranchesForUser)
                    .AddParameter("Username", username, DbType.String, size: 500)
                    .Query<Branch>();
        }
    }
}