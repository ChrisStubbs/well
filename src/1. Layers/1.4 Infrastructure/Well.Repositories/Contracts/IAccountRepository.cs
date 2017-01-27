namespace PH.Well.Repositories.Contracts
{
    using PH.Well.Domain;

    public interface IAccountRepository :IRepository<Account, int>
    {
        Account GetAccountByStopId(int stopId);

        Account GetAccountByAccountId(int accountId);

        Account GetAccountByAccountCode(string accountCode, int stopId);
    }
}
