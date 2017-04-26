namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;

    public interface IRouteReadRepository
    {
        IEnumerable<ReadRoute> GetAllRoutes(string username);
    }
}
