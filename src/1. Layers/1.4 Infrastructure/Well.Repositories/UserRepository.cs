﻿namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories.Contracts;

    public class UserRepository : DapperRepository<User, int>, IUserRepository
    {
        public UserRepository(ILogger logger, IDapperProxy dapperProxy, IUserNameProvider userNameProvider)
            : base(logger, dapperProxy, userNameProvider)
        {
        }

        public User GetById(int id)
        {
            return Get(id, null, null, null, null).SingleOrDefault();
        }

        public User GetByIdentity(string identity)
        {
            return Get(null, identity, null, null, null).SingleOrDefault();
        }

        public User GetByName(string name)
        {
            return Get(null, null, name, null, null).SingleOrDefault();
        }

        public IEnumerable<User> GetByBranchId(int branchId)
        {
            return Get(null, null, null, null, branchId);
        }

        public IEnumerable<decimal> GetCreditThresholds(string user)
        {
            return
                this.dapperProxy.WithStoredProcedure(StoredProcedures.UserGetCreditThreshold)
                    .AddParameter("Username", user, DbType.String)
                    .Query<decimal>();
        }

        public void SetThresholdLevel(User user, ThresholdLevel thresholdLevel)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.ThresholdLevelSave)
                .AddParameter("ThresholdLevelId", (int)thresholdLevel, DbType.Int32)
                .AddParameter("UserId", user.Id, DbType.Int32)
                .Execute();
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
                    .AddParameter("CreatedBy", this.CurrentUser, DbType.String, size: 50)
                    .AddParameter("DateCreated", now, DbType.DateTime)
                    .AddParameter("UpdatedBy", this.CurrentUser, DbType.String, size: 50)
                    .AddParameter("DateUpdated", now, DbType.DateTime)
                    .Execute();
        }

        public void UnAssignJobToUser(int jobId)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.UnAssignJobToUser)
                    .AddParameter("JobId", jobId, DbType.Int32)
                    .Execute();
        }

        public User GetUserByCreditThreshold(CreditThreshold creditThreshold)
        {
            return Get(null, null, null, creditThreshold.Id, null).FirstOrDefault();
        }

        private IEnumerable<User> Get(int? id, string identity, string name, int? creditThresholdId, int? branchId)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.UsersGet)
                .AddParameter("UserId", id, DbType.Int32)
                .AddParameter("Identity", identity, DbType.String, size: 255)
                .AddParameter("Name", name, DbType.String, size: 255)
                .AddParameter("CreditThresholdId", creditThresholdId, DbType.Int32)
                .AddParameter("BranchId", branchId, DbType.Int32)
                .Query<User>();

        }
    }
}