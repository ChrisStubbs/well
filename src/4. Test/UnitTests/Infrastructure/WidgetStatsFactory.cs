namespace PH.Well.UnitTests.Infrastructure
{
    using Domain.ValueObjects;
    using Factories;

    public class WidgetStatsFactory : EntityFactory<WidgetStatsFactory, WidgetStats>
    {
        public WidgetStatsFactory()
        {
            this.Entity.Id = 1;
        }
    }
}
