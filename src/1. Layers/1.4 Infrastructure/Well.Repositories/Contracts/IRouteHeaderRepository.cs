namespace PH.Well.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using Domain;

    public interface IRouteHeaderRepository: IRepository<RouteHeader, int>
    {
        IEnumerable<RouteHeader> GetRouteHeaders();

        Routes Create(Routes routes);

        Routes GetById(int id);

        bool FileAlreadyLoaded(string filename);

        void RouteHeaderCreateOrUpdate(RouteHeader routeHeader);

        RouteHeader GetRouteHeaderById(int id);

        RouteHeader GetRouteHeaderByRoute(string routeNumber, DateTime? routeDate);

        IEnumerable<RouteAttributeException> GetRouteAttributeException();

        void RoutesDeleteById(int id);

        IEnumerable<RouteHeader> GetRouteHeadersForDelete();

        IEnumerable<Routes> GetRoutes();

        void DeleteRouteHeaderById(int id);

        IEnumerable<RouteHeader> GetRouteHeadersGetByRoutesId(int routesId);
    }
}
