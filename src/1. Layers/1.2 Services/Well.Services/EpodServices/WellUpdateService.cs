namespace PH.Well.Services.EpodServices
{
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class WellUpdateService : IWellUpdateService
    {
        private readonly ILogger logger;

        private readonly IAccountRepository accountRepository;

        private readonly IRouteHeaderRepository routeHeaderRepository;

        private readonly IStopRepository stopRepository;

        private readonly IJobRepository jobRepository;

        private readonly IJobDetailRepository jobDetailRepository;

        public WellUpdateService(
            ILogger logger,
            IAccountRepository accountRepository,
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IJobRepository jobRepository,
            IJobDetailRepository jobDetailRepository)
        {
            this.logger = logger;
            this.accountRepository = accountRepository;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
        }

        public void UpdateWell()
        {
            this.logger.LogDebug("Start update of Well tables....");


            var locationAccounts = this.accountRepository.GetAccountsWithNoLocation().GroupBy(
                a => new {a.BranchId, a.AccountCode, a.Name, a.AddressLine1, a.AddressLine2, a.Postcode})
                .Select(l => new LocationAccount()
                {
                    BranchId = l.Key.BranchId,
                    AccountCode = l.Key.AccountCode,
                    AddressLine1 = l.Key.AddressLine1,
                    AddressLine2 = l.Key.AddressLine2,
                    Postcode = l.Key.Postcode
                });

            foreach (var location in locationAccounts)
            {
                // or can I pass in the table and do a merge into location
            }

            // select all stops with no location id
            //








        }
    }
}
