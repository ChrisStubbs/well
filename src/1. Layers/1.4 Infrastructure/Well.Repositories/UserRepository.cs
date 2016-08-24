using System;
using System.Collections.Generic;

namespace PH.Well.Repositories
{
    using System.Data;
    using System.Linq;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    public class UserRepository : DapperRepository<User, int>, IUserRepository
    {
        public UserRepository(ILogger logger, IDapperProxy dapperProxy)
            : base(logger, dapperProxy)
        {
        }

        public User GetByName(string name)
        {
            return
                this.dapperProxy.WithStoredProcedure(StoredProcedures.UserGetByName)
                    .AddParameter("Name", name, DbType.String, size: 255)
                    .Query<User>()
                    .SingleOrDefault();
        }

        public IEnumerable<User> GetByBranchId(int branchId)
        {
            return
                this.dapperProxy.WithStoredProcedure(StoredProcedures.UsersGetByBranchId)
                    .AddParameter("BranchId", branchId, DbType.Int32)
                    .Query<User>();
        }

        protected override void SaveNew(User entity)
        {
            entity.Id =
                this.dapperProxy.WithStoredProcedure(StoredProcedures.UserSave)
                    .AddParameter("Name", entity.Name, DbType.String, size: 255)
                    .AddParameter("JobDescription", entity.JobDescription, DbType.String, size: 500)
                    .AddParameter("IdentityName", entity.IdentityName, DbType.String, size: 255)
                    .AddParameter("Domain", entity.Domain, DbType.String, size: 50)
                    .AddParameter("CreatedBy", entity.CreatedBy, DbType.String, size: 50)
                    .AddParameter("DateCreated", entity.DateCreated, DbType.DateTime)
                    .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String, size: 50)
                    .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                    .Query<int>().SingleOrDefault();
        }

        public void AssignJobToUser(int userId, int jobId)
        {
            var now = DateTime.Now;
            this.dapperProxy.WithStoredProcedure(StoredProcedures.AssignJobToUser)
                    .AddParameter("UserId", userId, DbType.Int32)
                    .AddParameter("JobId", jobId, DbType.Int32)
                    .AddParameter("CreatedBy", "Test", DbType.String, size: 50)
                    .AddParameter("DateCreated", now, DbType.DateTime)
                    .AddParameter("UpdatedBy", "Test", DbType.String, size: 50)
                    .AddParameter("DateUpdated", now, DbType.DateTime)
                    .Query<int>();
        }
    }
}