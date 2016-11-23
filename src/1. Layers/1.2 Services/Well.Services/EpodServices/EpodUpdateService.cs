namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
    using System.Transactions;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class EpodUpdateService : IEpodUpdateService
    {
        private readonly ILogger logger;
        private readonly IEventLogger eventLogger;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IStopRepository stopRepository;
        private readonly IJobRepository jobRepository;

        private readonly IRouteMapper mapper;
        private const string UpdatedBy = "EpodUpdate";

        public EpodUpdateService(
            ILogger logger,
            IEventLogger eventLogger,
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IJobRepository jobRepository,
            IRouteMapper mapper)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.mapper = mapper;

            this.routeHeaderRepository.CurrentUser = UpdatedBy;
            this.stopRepository.CurrentUser = UpdatedBy;
            this.jobRepository.CurrentUser = UpdatedBy;
        }

        public void Update(RouteDelivery route)
        {
            foreach (var header in route.RouteHeaders)
            {
                var existingHeader = this.routeHeaderRepository.GetRouteHeaderByRoute(
                    header.RouteNumber,
                    header.RouteDate);

                if (existingHeader == null)
                {
                    logger.LogDebug($"No data found for Epod route: {header.RouteNumber} on date: {header.RouteDate}");
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"No data found for Epod route: {header.RouteNumber} on date: {header.RouteDate}",
                        7450);

                    continue;
                }

                this.mapper.Map(header, existingHeader);

                this.routeHeaderRepository.Update(existingHeader);

                this.UpdateStops(header.Stops);
            }
        }

        private void UpdateStops(IEnumerable<Stop> stops)
        {
            foreach (var stop in stops)
            {
                try
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        var existingStop = this.stopRepository.GetByTransportOrderReference(stop.TransportOrderReference);

                        if (existingStop == null)
                        {
                            this.logger.LogDebug($"Existing stop not found with transport order reference {stop.TransportOrderReference}");
                            this.eventLogger.TryWriteToEventLog(
                                EventSource.WellAdamXmlImport,
                                $"Existing stop not found with transport order reference {stop.TransportOrderReference}",
                                7666);

                            continue;
                        }

                        this.mapper.Map(stop, existingStop);

                        this.stopRepository.Update(existingStop);

                        this.UpdateJobs(stop.Jobs, existingStop.Id);

                        transactionScope.Complete();
                    }
                }
                catch (Exception exception)
                {
                    this.logger.LogError($"Stop has an error on Epod update! Stop Id ({stop.Id}), Transport order reference ({stop.TransportOrderReference})", exception);
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"Stop has an error on Epod update! Stop Id ({stop.Id}), Transport order reference ({stop.TransportOrderReference})",
                        9859);
                }
            }
        }

        private void UpdateJobs(IEnumerable<Job> jobs, int stopId)
        {
            foreach (var job in jobs)
            {
                var existingJob = this.jobRepository.GetByAccountPicklistAndStopId(
                    job.PhAccount,
                    job.PickListRef,
                    stopId);

                if (existingJob == null)
                {
                    this.logger.LogDebug($"Existing job not found for stop id ({stopId}), Account ({job.PhAccount}), Picklist ({job.PickListRef})");
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"Existing job not found for stop id ({stopId}), Account ({job.PhAccount}), Picklist ({job.PickListRef})",
                        7669);

                    continue;
                }
            }
        }
    }
}