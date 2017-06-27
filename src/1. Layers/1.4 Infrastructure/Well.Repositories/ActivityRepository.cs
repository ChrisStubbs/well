namespace PH.Well.Repositories
{
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Dapper;
    using Domain.ValueObjects;

    public class ActivityRepository : IActivityRepository
    {
        private readonly ILogger logger;
        private readonly IDapperReadProxy dapperReadProxy;

        public ActivityRepository(ILogger logger, IDapperReadProxy dapperReadProxy)
        {
            this.logger = logger;
            this.dapperReadProxy = dapperReadProxy;
        }

        public ActivitySource GetActivitySourceByDocumentNumber(string documentNumber)
        {
            var activitySource = new ActivitySource();

            dapperReadProxy.WithStoredProcedure(StoredProcedures.ActivityGetByDocumentNumber)
                .AddParameter("documentNumber", documentNumber, DbType.String)
                .QueryMultiple(x => activitySource = GetActivityFromGrid(x));

            return activitySource;
        }

        public ActivitySource GetActivityFromGrid(SqlMapper.GridReader grid)
        {
            var activitySource = grid.Read<ActivitySource>().FirstOrDefault();
            var details = grid.Read<ActivitySourceDetail>().ToList();
            if (activitySource != null)
            {
                activitySource.Details = details.Where(x => x.ActivityId == activitySource.ActivityId).ToList();
            }

            return activitySource;
        }
    }
}
