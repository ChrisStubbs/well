using PH.Well.Repositories.Contracts;

namespace PH.Well.Services
{
    using System.Collections.Generic;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;

    public class DeliveryService
    {
        public ILogger Logger { get; set; }

        public IRouteHeaderRepository RouteHeaderRepository { get; set; }

        public IStopRepository StopRepository { get; set; }

        public IJobRepository JobRepository { get; set; }

        public IAccountRepository AccountRepository { get; set; }

        public DeliveryService(ILogger logger, IRouteHeaderRepository routeHeaderRepository, IStopRepository stopRepository, IJobRepository jobRepository, IAccountRepository accountRepository)
        {
            this.Logger = logger;
            this.RouteHeaderRepository = routeHeaderRepository;
            this.StopRepository = stopRepository;
            this.JobRepository = jobRepository;
            this.AccountRepository = accountRepository;
        }






    }
}
