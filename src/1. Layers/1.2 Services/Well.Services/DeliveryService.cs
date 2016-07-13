using PH.Well.Repositories.Contracts;

namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Common.Contracts;
    using Domain.ValueObjects;

    using PH.Well.Services.Contracts;

    public class DeliveryService : IDeliveryService
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

        public IEnumerable<CleanDelivery> GetCleanDeliveries()
        {
            var status = 6; // complete
            var jobs = this.JobRepository.GetByStatus(status);
            var cleanDeliveries = new Collection<CleanDelivery>();

            foreach (var job in jobs)
            {
                var cleanDelivery = new CleanDelivery();

                cleanDelivery.InvoiceNumber = job.JobRef3;
                cleanDelivery.JobStatus = job.PerformanceStatusCode;
                cleanDelivery.AccountCode = job.JobRef1;

                var stop = this.StopRepository.GetById(job.StopId);
                cleanDelivery.DropId = stop.DropId;

                var route = this.RouteHeaderRepository.GetRouteHeaderById(int.Parse(stop.RouteId));
                cleanDelivery.RouteNumber = route.RouteNumber;

                var account = this.AccountRepository.GetAccountByStopId(job.StopId);
                cleanDelivery.AccountName = account.Name;

                cleanDeliveries.Add(cleanDelivery);
            }

            return cleanDeliveries;
        }





    }
}
