namespace PH.Well.Api.Infrastructure
{
    using Models;
    using Repositories;
    using Repositories.Contracts;
    using Shared.Well.Data.EF.Contracts;

    public class WelllApiDbConfiguration : BaseDbConfiguration, IWellDbConfiguration, IWellEntitiesConnectionString
    {
        private readonly IBranchProvider branchProvider;
        private readonly IConnectionStringFactory connectionStringFactory;

        public WelllApiDbConfiguration(IBranchProvider branchProvider, IConnectionStringFactory connectionStringFactory)
        {
            this.branchProvider = branchProvider;
            this.connectionStringFactory = connectionStringFactory;
            this.NameOrConnectionString = connectionStringFactory.GetConnectionString(branchProvider.GetBranchId(), ConnectionType.Ef); ;
        }

        public string DatabaseConnection => connectionStringFactory.GetConnectionString(branchProvider.GetBranchId(), ConnectionType.Dapper);
        public string NameOrConnectionString { get; set; }
    }
}