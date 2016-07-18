namespace PH.Well.Repositories
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


    }
}
