namespace PH.Well.UnitTests.Factories
{
    using Domain.ValueObjects;

    public class WidgetStatsFactory : EntityFactory<WidgetStatsFactory, WidgetStats>
    {
        public WidgetStatsFactory()
        {
            this.Entity.Id = 1;
        }
    }
}
