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

        public ActivitySource GetActivitySourceByDocumentNumber(string documentNumber, int branchId)
        {
            var activitySource = new ActivitySource();

            dapperReadProxy.WithStoredProcedure(StoredProcedures.ActivityGetByDocumentNumber)
                .AddParameter("documentNumber", documentNumber, DbType.String)
                .AddParameter("branchId", branchId, DbType.Int32)
                .QueryMultiple(x => activitySource = GetActivityFromGrid(x));

            return activitySource;
        }

        public ActivitySource GetActivityFromGrid(SqlMapper.GridReader grid)
        {
            var activitySource = grid.Read<ActivitySource>().FirstOrDefault();
            var details = grid.Read<ActivitySourceDetail>().ToList();
            var assignees = grid.Read<string>().ToList();
            if (activitySource != null)
            {
                activitySource.Details = details.Where(x => x.ActivityId == activitySource.ActivityId).ToList();
                activitySource.Assignees = assignees;
            }

            return activitySource;
        }
    }
}
