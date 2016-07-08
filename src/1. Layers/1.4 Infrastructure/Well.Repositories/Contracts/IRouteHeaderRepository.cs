namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;
    public interface IRouteHeaderRepository: IRepository<RouteHeader, int>
    {
        IEnumerable<RouteHeader> GetRouteHeaders();

        Routes CreateOrUpdate(Routes routes);

        Routes GetById(int id);

        Routes GetByFilename(string filename);
    }
}
