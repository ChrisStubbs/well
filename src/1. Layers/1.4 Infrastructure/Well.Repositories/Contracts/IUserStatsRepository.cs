namespace PH.Well.Repositories.Contracts
{
    using Domain.ValueObjects;

    public interface IUserStatsRepository
    {
        UserStats GetByUser(string userIdentity);

        int GetPendingCreditCountByUser(string userIdentity);
    }
}