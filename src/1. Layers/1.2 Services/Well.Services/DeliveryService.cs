using PH.Well.Repositories.Contracts;

namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Common.Contracts;
    using Common.Extensions;
    using Domain;
    using Domain.ValueObjects;
    using Domain.Enums;

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

        public IEnumerable<Delivery> GetCleanDeliveries()
        {
            return GetDeliveriesByStatus(PerformanceStatus.Compl);
        }

        public IEnumerable<Delivery> GetResolvedDeliveries()
        {
            //Todo this is not right
            return GetDeliveriesByStatus(PerformanceStatus.Incom);
        }

        private IEnumerable<Delivery> GetDeliveriesByStatus(PerformanceStatus status)
        {
            var jobs = this.JobRepository.GetByStatus(status);
            var cleanDeliveries = new Collection<Delivery>();
            var routes = new Collection<RouteHeader>();
            var accounts = new Collection<Account>();

            foreach (var job in jobs)
            {
                var cleanDelivery = new Delivery();

                cleanDelivery.InvoiceNumber = job.JobRef3;
                cleanDelivery.JobStatus =  StringExtensions.GetEnumDescription((PerformanceStatus)job.PerformanceStatusId);
                cleanDelivery.AccountCode = job.JobRef1;

                var stop = this.StopRepository.GetById(job.StopId);
                cleanDelivery.DropId = stop.DropId;

                var routeId = stop.RouteHeaderId;
                var route = routes.FirstOrDefault(x => x.RoutesId == routeId);

                if (route == null)
                {
                    route = this.RouteHeaderRepository.GetRouteHeaderById(stop.RouteHeaderId);
                    routes.Add(route);
                }
                cleanDelivery.RouteNumber = route.RouteNumber;

                var account = accounts.FirstOrDefault(x => x.StopId == job.StopId);

                if (account == null)
                {
                    account = this.AccountRepository.GetAccountByStopId(job.StopId);
                    accounts.Add(account);
                }

                cleanDelivery.AccountName = account.Name;
                cleanDeliveries.Add(cleanDelivery);
            }

            return cleanDeliveries;
        }

       
    }
}
