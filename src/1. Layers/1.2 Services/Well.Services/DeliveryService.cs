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
            //Todo this is not right what is the status For Resolved!!
            return GetDeliveriesByStatus(PerformanceStatus.Incom);
        }

        private IEnumerable<Delivery> GetDeliveriesByStatus(PerformanceStatus status)
        {

            var jobs = this.JobRepository.GetByStatus(status);

            var deliveries = new Collection<Delivery>();
            //these are used to cache data once retrieved from dtabase my be more performant just to get the hole lot up front
            var routes = new Collection<RouteHeader>();
            var accounts = new Collection<Account>();

            foreach (var job in jobs)
            {
                var delivery = new Delivery();

                delivery.InvoiceNumber = job.JobRef3;
                delivery.JobStatus =  StringExtensions.GetEnumDescription((PerformanceStatus)job.JobPerformanceStatusCodeId);
                delivery.AccountCode = job.JobRef1;
                
                var stop = this.StopRepository.GetById(job.StopId);
                delivery.DropId = stop.DropId;

                var routeId = stop.RouteHeaderId;
                var route = routes.FirstOrDefault(x => x.RoutesId == routeId);

                if (route == null)
                {
                    route = this.RouteHeaderRepository.GetRouteHeaderById(stop.RouteHeaderId);
                    routes.Add(route);
                }
                delivery.RouteNumber = route.RouteNumber;

                var account = accounts.FirstOrDefault(x => x.StopId == job.StopId);

                if (account == null)
                {
                    account = this.AccountRepository.GetAccountByStopId(job.StopId);
                    accounts.Add(account);
                }

                delivery.AccountName = account.Name;
                deliveries.Add(delivery);
            }

            return deliveries;
        }

       
    }
}
