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

        public AppSearchReadRepository(ILogger logger, IDapperReadProxy dapperReadProxy)
        {
            this.logger = logger;
            this.dapperReadProxy = dapperReadProxy;
        }

        public IEnumerable<AppSearchResult> Search(AppSearchParameters searchParameters)
        {


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
