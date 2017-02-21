namespace PH.Well.Repositories.Contracts
{
    using Domain.ValueObjects;

    public interface IUserStatsRepository
    {
        WidgetWarningLevels GetWidgetWarningLevels(string userIdentity);
    }
}