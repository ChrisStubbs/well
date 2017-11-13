namespace PH.Well.Api.Infrastructure
{
    using Repositories;
    using Repositories.Contracts;

    public class WelllApiDbConfiguration : BaseDbConfiguration, IWellDbConfiguration
    {
        private readonly IBranchProvider branchProvider;
        private readonly IConnectionStringFactory connectionStringFactory;

        public WelllApiDbConfiguration(IBranchProvider branchProvider, IConnectionStringFactory connectionStringFactory)
        {
            this.branchProvider = branchProvider;
            this.connectionStringFactory = connectionStringFactory;
        }

        public string DatabaseConnection => connectionStringFactory.GetConnectionString(branchProvider.GetBranchId());
    }
}