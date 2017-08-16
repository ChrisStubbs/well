using PH.Shared.Well.Data.EF;

namespace PH.Well.Repositories.Read
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Dapper;
    using Domain.ValueObjects;

    public class AppSearchReadRepository : IAppSearchReadRepository
    {
        private readonly ILogger logger;
        private readonly IDapperReadProxy dapperReadProxy;
        private readonly WellEntities wellEntities;

        public AppSearchReadRepository(ILogger logger, IDapperReadProxy dapperReadProxy, WellEntities wellEntities)
        {
            this.logger = logger;
            this.dapperReadProxy = dapperReadProxy;
            this.wellEntities = wellEntities;
        }

        //public IEnumerable<AppSearchResult> Search(AppSearchParameters searchParameters)
        //{
        //    return dapperReadProxy.WithStoredProcedure(StoredProcedures.AppSearch)
        //        .AddParameter("BranchId", searchParameters.BranchId, DbType.Int32)
        //        .AddParameter("Date", searchParameters.Date, DbType.Date)
        //        .AddParameter("Account", string.IsNullOrWhiteSpace(searchParameters.Account) ? null : searchParameters.Account, DbType.String)
        //        .AddParameter("Invoice", string.IsNullOrWhiteSpace(searchParameters.Invoice) ? null : searchParameters.Invoice, DbType.String)
        //        .AddParameter("Route", string.IsNullOrWhiteSpace(searchParameters.Route) ? null : searchParameters.Route, DbType.String)
        //        .AddParameter("Driver", string.IsNullOrWhiteSpace(searchParameters.Driver) ? null : searchParameters.Driver, DbType.String)
        //        .AddParameter("DeliveryType", searchParameters.DeliveryType, DbType.Int32)
        //        .AddParameter("Status", searchParameters.Status, DbType.Int32)
        //        .Query<AppSearchResult>();
        //}
        public IEnumerable<AppSearchResult> Search(AppSearchParameters searchParameters)
        {
            searchParameters.Format();
            var results = new List<AppSearchResult>();

            // Should always have a branch
            if (searchParameters.HasBranch)
            {
                // Start with all routes for the selected branch
                var routes = wellEntities.RouteHeader.Where(x => x.RouteOwnerId == searchParameters.BranchId.Value).AsQueryable();
                
                // Apply any route filter first to reduce matches
                if (searchParameters.HasRoute)
                {
                    routes = routes.Where(x => x.RouteNumber == searchParameters.Route);
                }

                // A search by driver looks at routes first
                if (String.IsNullOrEmpty(searchParameters.Driver))
                {
                    routes = routes.Where(x => x.DriverName == searchParameters.Driver);
                }

                // If a date is supplied, narrow the search
                if (searchParameters.HasDate)
                {
                    routes = routes.Where(x => x.RouteDate == searchParameters.Date.Value);
                }

                if (searchParameters.HasDeliveryType)
            }
            return results;

            return dapperReadProxy.WithStoredProcedure(StoredProcedures.AppSearch)
                .AddParameter("BranchId", searchParameters.BranchId, DbType.Int32)
                .AddParameter("Date", searchParameters.Date, DbType.Date)
                .AddParameter("Account", string.IsNullOrWhiteSpace(searchParameters.Account) ? null : searchParameters.Account, DbType.String)
                .AddParameter("Invoice", string.IsNullOrWhiteSpace(searchParameters.Invoice) ? null : searchParameters.Invoice, DbType.String)
                .AddParameter("Route", string.IsNullOrWhiteSpace(searchParameters.Route) ? null : searchParameters.Route, DbType.String)
                .AddParameter("Driver", string.IsNullOrWhiteSpace(searchParameters.Driver) ? null : searchParameters.Driver, DbType.String)
                .AddParameter("DeliveryType", searchParameters.DeliveryType, DbType.Int32)
                .AddParameter("Status", searchParameters.Status, DbType.Int32)
                .Query<AppSearchResult>();
        }
    }
}
