namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;
    using PH.Well.Domain;

    public interface IAccountRepository :IRepository<Account, int>
    {
        Account GetAccountByStopId(int stopId);

        Account GetAccountByAccountId(int accountId);

    }
}
