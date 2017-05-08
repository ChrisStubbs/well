namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;

    public interface IAssigneeReadRepository
    {
        IEnumerable<Assignee> GetByRouteHeaderId(int routeHeaderId);
    }
}