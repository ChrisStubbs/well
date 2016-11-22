﻿namespace PH.Well.Repositories
{
    using System.Data;
    using System.Linq;

    using Domain;
    using Contracts;

    using Common.Contracts;

    public class AccountRepository : DapperRepository<Account, int>, IAccountRepository
    {

        public AccountRepository(ILogger logger, IWellDapperProxy dapperProxy)
            : base(logger, dapperProxy)
        {
        }

        public Account GetAccountByStopId(int stopId)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.AccountGetByStopId)
                    .AddParameter("StopId", stopId, DbType.Int32)
                    .Query<Account>()
                    .FirstOrDefault();

        }

        public Account GetAccountByAccountId(int accountId)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.AccountGetByAccountId)
                    .AddParameter("AccountId", accountId, DbType.Int32)
                    .Query<Account>()
                    .FirstOrDefault();

        }

        public Account GetAccountGetByAccountCode(string accountCode, int stopId)
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.AccountGetByAccountCode)
                    .AddParameter("Code", accountCode, DbType.String)
                    .AddParameter("StopId", stopId, DbType.Int32)
                    .Query<Account>()
                    .FirstOrDefault();
        }

        protected override void SaveNew(Account entity)
        {
            entity.Id = this.dapperProxy.WithStoredProcedure(StoredProcedures.AccountInsert)
                .AddParameter("Code", entity.Code, DbType.String)
                .AddParameter("AccountTypeCode", entity.AccountTypeCode, DbType.String)
                .AddParameter("DepotId", entity.DepotId, DbType.Int32)
                .AddParameter("Name", entity.Name, DbType.String)
                .AddParameter("Address1", entity.Address1, DbType.String)
                .AddParameter("Address2", entity.Address2, DbType.String)
                .AddParameter("PostCode", entity.PostCode, DbType.String)
                .AddParameter("ContactName", entity.ContactName, DbType.String)
                .AddParameter("ContactNumber", entity.ContactNumber, DbType.String)
                .AddParameter("ContactNumber2", entity.ContactNumber2, DbType.String)
                .AddParameter("ContactEmailAddress", entity.ContactEmailAddress, DbType.String)
                .AddParameter("StopId", entity.StopId, DbType.Int32)
                .AddParameter("CreatedBy", entity.CreatedBy, DbType.String)
                .AddParameter("UpdatedBy", entity.UpdatedBy, DbType.String)
                .AddParameter("CreatedDate", entity.DateCreated, DbType.DateTime)
                .AddParameter("UpdatedDate", entity.DateUpdated, DbType.DateTime).Query<int>().FirstOrDefault();
        }
    }
}
