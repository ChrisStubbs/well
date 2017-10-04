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
        public IEnumerable<AppSearchResult> Search(AppSearchParameters searchParameters)
        {
            searchParameters.Format();
            var results = new List<AppSearchResult>();

            // Should always have a branch
            if (searchParameters.HasBranch)
            {
                var branchId = searchParameters.BranchId.Value;
                // If an invoice number is supplied, see if that activity exists
                if (searchParameters.HasInvoice)
                {
                    ActivitySearch(searchParameters, branchId, results, Domain.Enums.ActivityType.Invoice);
                }

                if (searchParameters.HasUpliftInvoiceNumber)
                {
                    ActivitySearch(searchParameters, branchId, results, Domain.Enums.ActivityType.Uplift);
                }

                // If an account number exists, find matching accounts
                if (searchParameters.HasAccount)
                {
                    var accounts = wellEntities.Account.Where(x => x.Code == searchParameters.Account && x.Location.BranchId == branchId);
                    if (searchParameters.HasDate)
                    {
                        accounts =
                            accounts.Where(x => x.Stop.Job.Any(y => y.Stop.DeliveryDate == searchParameters.Date.Value));
                    }
                    foreach (var account in accounts)
                    {
                        results.Add(new AppSearchResult()
                        {
                            BranchId = searchParameters.BranchId,
                            LocationId = account.LocationId
                        });
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
                    foreach (var account in routes /*.Select(x=> new {x.StopId})*/)
                    {
                        results.Add(new AppSearchResult()
                        {
                            BranchId = searchParameters.BranchId,
                            RouteId = account.Id,
                        });
                    }
                }
            }

            return results;
        }

        private void ActivitySearch(AppSearchParameters searchParameters, int branchId, List<AppSearchResult> results, Domain.Enums.ActivityType activityType)
        {
            var documentNumber = string.Empty;
            documentNumber = activityType == ActivityType.Uplift ? searchParameters.UpliftInvoiceNumber : searchParameters.Invoice;

            var invoices = wellEntities.Activity
                .Where(x => x.DocumentNumber == documentNumber
                            && x.ActivityTypeId == (byte)activityType
                            && x.Location.BranchId == branchId);

            if (searchParameters.HasDate)
            {
                invoices = invoices
                    .Where(x => x.Job.Any(y => y.Stop.DeliveryDate == searchParameters.Date.Value));
            }
            foreach (var activity in invoices)
            {
                results.Add(new AppSearchResult()
                {
                    BranchId = searchParameters.BranchId,
                    InvoiceId = activity.Id,
                });
            }
        }
    }
}
