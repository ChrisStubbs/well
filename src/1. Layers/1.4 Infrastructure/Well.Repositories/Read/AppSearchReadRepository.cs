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
                    var invoices = wellEntities.Activity.Where(x => x.DocumentNumber == searchParameters.Invoice && x.Location.BranchId == branchId);
                    if (searchParameters.HasDate)
                    {
                        invoices =
                            invoices.Where(x => x.Job.Any(y => y.Stop.DeliveryDate == searchParameters.Date.Value));
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

                // If an account number exists, find matching accounts
                if (searchParameters.HasAccount)
                {
                    var accounts = wellEntities.Account.Where(x => x.Code == searchParameters.Account && x.Location.BranchId == branchId);
                    if (searchParameters.HasDate)
                    {
                        accounts =
                            accounts.Where(x => x.Stop.Job.Any(y => y.Stop.DeliveryDate == searchParameters.Date.Value));
                    }
                    foreach (var account in accounts /*.Select(x=> new {x.StopId})*/)
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
                            RouteId = account.RoutesId,
                        });
                    }
                }

                // DIJ: Not currently using delivery type or job type searches
                //if (searchParameters.HasDeliveryType)
                //{
                //    var jobTypeCode =
                //        wellEntities.JobType.FirstOrDefault(x => x.Id == searchParameters.DeliveryType.Value);
                //    routes = routes.Where(x => x.Stop.Any(y => y.Job.Any(z => z.JobTypeCode == jobTypeCode.Code)));
                //}

                //if (searchParameters.HasStatus)
                //{
                //    var status = wellEntities.WellStatus.FirstOrDefault(x => x.Id == (byte) searchParameters.Status.Value);
                //    routes = routes.Where(x=>x.Well)
                //}
            }
            return results;
        }
    }
}
