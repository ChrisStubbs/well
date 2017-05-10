namespace PH.Well.Repositories.Read
{
    using System.Collections.Generic;
    using System.Data;
    using Contracts;
    using Domain.ValueObjects;

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
    }
}