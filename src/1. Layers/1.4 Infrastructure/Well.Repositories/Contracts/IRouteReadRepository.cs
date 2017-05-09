namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;

    public interface IRouteReadRepository
    {
        IEnumerable<Route> GetAllRoutes(string username);
    }
}
