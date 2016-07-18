namespace PH.Well.UnitTests.Factories
{
    using Well.Domain.ValueObjects;

    public class WidgetStatsFactory : EntityFactory<WidgetStatsFactory, WidgetStats>
    {
        public WidgetStatsFactory()
        {
            this.Entity.Id = 1;
        }
    }
}
