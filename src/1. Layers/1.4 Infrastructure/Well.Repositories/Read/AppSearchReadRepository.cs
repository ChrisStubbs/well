using System.Data.Entity;
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
    using Domain.Enums;
    using Domain.ValueObjects;

    public class AppSearchReadRepository : IAppSearchReadRepository
    {
        private readonly ILogger logger;
        private readonly IDapperReadProxy dapperReadProxy;
        private readonly WellEntities wellEntities;
        private int maxResultsPerType = 5;

        public AppSearchReadRepository(ILogger logger, IDapperReadProxy dapperReadProxy, WellEntities wellEntities)
        {
            this.logger = logger;
            this.dapperReadProxy = dapperReadProxy;
            this.wellEntities = wellEntities;
        }

        /// <summary>
        /// Provide search results based search criteria
        /// </summary>
        /// <param name="searchParameters"></param>
        /// <returns></returns>
        /// <remarks>
        /// Valid combinations are:
        ///     Branch 
        ///         + Invoice number
        ///         + Account number
        ///         + Driver name
        ///         + Route
        ///     [Any] + Date
        /// </remarks>
        public IEnumerable<AppSearchItem> Search(AppSearchParameters searchParameters)
        {
            searchParameters.Format();
            var results = new List<AppSearchItem>();

            // Should always have a branch
            if (searchParameters.HasBranch)
            {
                var branchId = searchParameters.BranchId.Value;
                // If an invoice number is supplied, see if that activity exists
                if (searchParameters.HasInvoice)
                {
                    ActivitySearch(SearchActivityByDocumentNumber(searchParameters, branchId), results);
                }

                if (searchParameters.HasUpliftInvoiceNumber)
                {
                    ActivitySearch(SearchActivityByInitialDocumentNumber(searchParameters, branchId), results);
                }

                // If an account number exists, find matching accounts
                if (searchParameters.HasAccount)
                {
                    var locations = wellEntities.Location.Where(x => x.AccountCode == searchParameters.Account && x.BranchId == branchId);
                    if (searchParameters.HasDate)
                    {
                        locations =
                            locations.Where(x => x.Stop.Any(
                                s => s.Job.Any(y => y.Stop.DeliveryDate == searchParameters.Date.Value)));
                    }
                    foreach (var location in locations.Take(maxResultsPerType))
                    {
                        results.Add(new AppSearchLocationItem(location.Id, location.Name, location.AccountCode));
                    }
                }

                if (searchParameters.IsRouteSearch)
                {
                    // Start with all routes for the selected branch
                    var routes = wellEntities.RouteHeader.Where(x => x.RouteOwnerId == branchId).AsQueryable();

                    // Apply any route filter first to reduce matches
                    if (searchParameters.HasRoute)
                    {
                        routes = routes.Where(x => x.RouteNumber == searchParameters.Route);
                    }

                    // A search by driver looks at routes first
                    if (searchParameters.HasDriver)
                    {
                        routes = routes.Where(x => x.DriverName == searchParameters.Driver);
                    }

                    // If a date is supplied, narrow the search
                    if (searchParameters.HasDate)
                    {
                        routes = routes.Where(x => x.RouteDate == searchParameters.Date.Value);
                    }
                    foreach (var route in routes.Take(maxResultsPerType))
                    {
                        results.Add(new AppSearchRouteItem(route.Id,route.RouteNumber, route.DriverName,
                            route.RouteDate.GetValueOrDefault()));
                    }
                }
            }

            return results;
        }

        private IQueryable<Activity> SearchActivityByInitialDocumentNumber(AppSearchParameters searchParameters, int branchId)
        {
            var query = wellEntities.Activity
                .Where(x => x.InitialDocument == searchParameters.UpliftInvoiceNumber
                            && x.Location.BranchId == branchId);

            if (searchParameters.HasDate)
            {
                query = query
                    .Where(x => x.Job.Any(y => y.Stop.DeliveryDate == searchParameters.Date.Value));
            }

            return query;
        }

        private IQueryable<Activity> SearchActivityByDocumentNumber(AppSearchParameters searchParameters, int branchId)
        {
            var query = wellEntities.Activity
                .Where(x => x.DocumentNumber == searchParameters.Invoice
                            && x.Location.BranchId == branchId);

            if (searchParameters.HasDate)
            {
                query = query
                    .Where(x => x.Job.Any(y => y.Stop.DeliveryDate == searchParameters.Date.Value));
            }

            return query;
        }

        private void ActivitySearch(IQueryable<Activity> query, List<AppSearchItem> results)
        {
            var invoices = query.Select(x => new
            {
                x.Id,
                x.DocumentNumber,
                x.ActivityTypeId,
                Date = x.Job.FirstOrDefault(j => j.DateDeleted == null).Stop.RouteHeader.RouteDate,
                AccountName = x.Location.Name
            }).Take(maxResultsPerType).ToList();

            foreach (var activity in invoices)
            {
                results.Add(new AppSearchInvoiceItem(activity.Id, activity.DocumentNumber,
                    ((ActivityType)activity.ActivityTypeId).ToString(), activity.AccountName,
                    activity.Date.GetValueOrDefault()));
            }
        }
    }
}
