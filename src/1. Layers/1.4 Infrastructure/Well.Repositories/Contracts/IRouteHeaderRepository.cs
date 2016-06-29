namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;
    public interface IRouteHeaderRepository: IRepository<RouteHeader, int>
    {
        IEnumerable<RouteHeader> GetRouteHeaders();
    }
}
