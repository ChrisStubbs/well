namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain.ValueObjects;

    public interface IRouteToRemoveRepository
    {
        IEnumerable<int> GetRouteIds();

        RouteToRemove GetRouteToRemove(int routeId);
    }
}