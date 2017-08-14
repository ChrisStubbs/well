namespace PH.Well.Repositories.Read
{
    using System.Collections.Generic;
    using System.Data;
    using Contracts;
    using Domain.ValueObjects;
    using System.Linq;
    using Common.Extensions;

    public class AssigneeReadRepository : IAssigneeReadRepository
    {
        private readonly IDapperReadProxy dapperReadProxy;

        public AssigneeReadRepository(IDapperReadProxy dapperReadProxy)
        {
            this.dapperReadProxy = dapperReadProxy;
        }

        public IEnumerable<Assignee> GetByRouteHeaderId(int routeHeaderId)
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.AssigneeGetByRouteHeaderId)
                    .AddParameter("RouteHeaderId", routeHeaderId, DbType.Int32)
                    .Query<Assignee>();
        }

        public IEnumerable<Assignee> GetByStopId(int stopId)
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.AssigneeGetByStopId)
                    .AddParameter("StopId", stopId, DbType.Int32)
                    .Query<Assignee>();
        }

        public Assignee GetByJobId(int jobId)
        {
            return GetByJobIds(new[] {jobId}).SingleOrDefault();
        }

        public IEnumerable<Assignee> GetByJobIds(IEnumerable<int> jobIds)
        {
            return dapperReadProxy.WithStoredProcedure(StoredProcedures.AssigneeGetByJobIds)
                .AddParameter("JobIds", jobIds.Distinct().ToList().ToIntDataTables("Ids"), DbType.Object)
                .Query<Assignee>();
        }
    }
}