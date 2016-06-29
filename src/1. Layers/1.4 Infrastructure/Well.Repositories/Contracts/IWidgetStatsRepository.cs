namespace PH.Well.Repositories.Contracts
{
    using Domain.ValueObjects;

    public  interface IWidgetStatsRepository : IRepository<WidgetStats, int>
    {
        WidgetStats GetWidgetStats();
    }
}