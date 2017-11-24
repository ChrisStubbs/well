namespace PH.Well.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Transactions;
    using Domain.Enums;
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
           Save(entity, dapperProxy.DbConfiguration.DatabaseConnection);
        }

        public void Save(CreditThreshold entity, string connectionString)
        {
            entity.SetCreatedProperties(this.CurrentUser);
            entity.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdSave)
                .AddParameter("Level", (int)entity.Level, DbType.Int32)
                .AddParameter("Value", entity.Threshold, DbType.Decimal)
                .AddParameter("DateCreated", entity.DateCreated, DbType.DateTime)
                .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String, size: 50)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String, size: 50)
                .Query<int>().Single();
        }

        public IEnumerable<CreditThreshold> GetAll()
        {
          return GetAll(dapperProxy.DbConfiguration.DatabaseConnection);
        }

        public IEnumerable<CreditThreshold> GetAll(string connection)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdGetAll).Query<CreditThreshold>(connection);
        }

        protected override void UpdateExisting(CreditThreshold entity)
        {
            Update(entity, dapperProxy.DbConfiguration.DatabaseConnection);
        }

        public void Update(CreditThreshold entity, string connectionString)
        {
            entity.SetUpdatedProperties(CurrentUser);
            dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdUpdate)
                .AddParameter("Id", entity.Id, DbType.Int32)
                .AddParameter("Threshold", entity.Threshold, DbType.Decimal)
                .AddParameter("DateUpdated", entity.DateUpdated, DbType.DateTime)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String, 50)
                .Execute(connectionString);
        }

        public void Delete(int id, string connectionString)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdDelete)
                .AddParameter("Id", id, DbType.Int32).Execute(connectionString);
        }

        public CreditThreshold GetById(int thresholdId, string connectionString)
        {
            return GetAll(connectionString)
                .SingleOrDefault(x => x.Id == thresholdId);
        }

        public CreditThreshold GetByLevel(ThresholdLevel level, string connectionString)
        {
            return GetAll(connectionString)
                .SingleOrDefault(x => x.Level == level);
        }

        public void PendingCreditInsert(int jobId)
        {
            this.dapperProxy.WithStoredProcedure(StoredProcedures.PendingCreditInsert)
                .AddParameter("jobId", jobId, DbType.Int32)
                .AddParameter("originator", this.CurrentUser, DbType.String)
                .Execute();
        }


        public CreditThreshold GetByUserId(int userId)
        {
            return dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdGetByUser)
                .AddParameter("UserId", userId, DbType.Int32)
                .Query<CreditThreshold>()
                .FirstOrDefault();
        }

        public void SetForUser(int userId, int creditThresholdId,string connectionString)
        {
            // Delete previously assigned threshold
            dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdUserDelete)
                .AddParameter("UserId", userId, DbType.Int32).Execute(connectionString);

            var now = DateTime.Now;
            var user = CurrentUser;
            dapperProxy.WithStoredProcedure(StoredProcedures.CreditThresholdUserInsert)
                .AddParameter("UserId", userId, DbType.Int32)
                .AddParameter("CreditThresholdId", creditThresholdId, DbType.Int32)
                .AddParameter("DateCreated", now, DbType.DateTime)
                .AddParameter("DateUpdated", now, DbType.DateTime)
                .AddParameter("CreatedBy", user, DbType.String, size: 50)
                .AddParameter("UpdatedBy", user, DbType.String, size: 50).Execute(connectionString);
        }
    }
}
