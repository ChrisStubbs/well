namespace PH.Well.Repositories
{
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain.ValueObjects;

    public class WidgetStatsRepository : DapperRepository<WidgetStats, int>, IWidgetStatsRepository
    {
        public WidgetStatsRepository(ILogger logger, IWellDapperProxy dapperProxy)
            : base(logger, dapperProxy)
        {
        }

        public WidgetStats GetWidgetStats()
        {
            return this.dapperProxy.WithStoredProcedure(StoredProcedures.WidgetStatsGet)
                                    .Query<WidgetStats>().SingleOrDefault();
        }
    }
}
