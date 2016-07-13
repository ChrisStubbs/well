namespace PH.Well.Repositories.Contracts
{
    using PH.Well.Domain;

    public interface IAccountRepository :IRepository<Account, int>
    {
        Account GetAccountByStopId(int stopId);
    }
}
