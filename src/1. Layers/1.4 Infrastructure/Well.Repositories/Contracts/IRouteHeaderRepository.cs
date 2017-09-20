namespace PH.Well.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Domain.Enums;

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
        
        IEnumerable<RouteHeader> GetRouteHeadersGetByRoutesId(int routesId);

        IList<GetByNumberDateBranchResult> GetByNumberDateBranch(IList<GetByNumberDateBranchFilter> filter);

        void DeleteRouteHeaderWithNoStops();

        void UpdateWellStatus(RouteHeader routeHeader);

        void UpdateFieldsFromImported(RouteHeaderFromImportedFile newData);
    }

    public struct GetByNumberDateBranchFilter
    {
        public string RouteNumber { get; set; }
        public DateTime RouteDate { get; set; }
        public int BranchId { get; set; }
    }

    public class GetByNumberDateBranchResult
    {
        public int Id { get; set; }
        public WellStatus WellStatus { get; set; }
        public string RouteNumber { get; set; }
        public DateTime RouteDate { get; set; }
        public int BranchId { get; set; }
    }

    public class RouteHeaderFromImportedFile
    {
        public int Id { get; set; }
        public int StartDepot { get; set; }
        public DateTime? RouteDate { get; set; }
        public string RouteNumber { get; set; }
        public int PlannedStops { get; set; }
        public int RouteOwnerId { get; set; }
    }

}
