namespace PH.Well.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using Domain;

    public interface IRouteHeaderRepository : IRepository<RouteHeader, int>
    {
        IEnumerable<RouteHeader> GetRouteHeaders();

        Routes Create(Routes routes);

        Routes GetById(int id);

        bool FileAlreadyLoaded(string filename);

        RouteHeader GetRouteHeaderById(int id);

        RouteHeader GetRouteHeaderByRoute(int branchId, string routeNumber, DateTime? routeDate);

        IEnumerable<RouteAttributeException> GetRouteAttributeException();

        void RoutesDeleteById(int id);

        IEnumerable<Routes> GetRoutes();

        void DeleteRouteHeaderById(int id);

        IEnumerable<RouteHeader> GetRouteHeadersGetByRoutesId(int routesId);

        RouteHeader GetByNumberDateBranch(string routeNumber, DateTime routeDate, int branchId);
    }
}
