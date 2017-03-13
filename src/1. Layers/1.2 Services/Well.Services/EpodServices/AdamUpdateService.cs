namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Common.Extensions;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class AdamUpdateService : IAdamUpdateService
    {
        private readonly ILogger logger;
        private readonly IEventLogger eventLogger;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IStopRepository stopRepository;
        private readonly IJobRepository jobRepository;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IRouteMapper mapper;
        private readonly IJobStatusService jobStatusService;

        public AdamUpdateService(
            ILogger logger,
            IEventLogger eventLogger,
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IJobRepository jobRepository,
            IJobDetailRepository jobDetailRepository,
            IRouteMapper mapper,
            IJobStatusService jobStatusService)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.mapper = mapper;
            this.jobStatusService = jobStatusService;
        }

        public void Update(RouteUpdates route)
        {
            foreach (var stop in route.Stops)
            {
                var action = GetOrderUpdateAction(stop.ActionIndicator);

                switch (action)
                {
                    case OrderActionIndicator.Insert:
                        this.Insert(stop);
                        break;
                    case OrderActionIndicator.Update:
                        this.Update(stop);
                        break;
                    case OrderActionIndicator.Delete:
                        this.Delete(stop);
                        break;
                }
            }
        }

        private void Insert(StopUpdate stop)
        {
            var existingRouteHeader = this.routeHeaderRepository.GetByNumberDateBranch(
                stop.RouteNumber,
                stop.DeliveryDate.Value,
                int.Parse(stop.StartDepotCode));

            if (existingRouteHeader == null)
            {
                this.logger.LogDebug($"Existing route header not found for route number ({stop.RouteNumber}), delivery date ({stop.DeliveryDate})!");
                this.eventLogger.TryWriteToEventLog(
                    EventSource.WellAdamXmlImport,
                    $"Existing route header not found for route number ({stop.RouteNumber}), delivery date ({stop.DeliveryDate})!",
                    3215);
                return;
            }

            this.InsertStops(stop, existingRouteHeader);
        }

        private void Update(StopUpdate stop)
        {
            var job = stop.Jobs.First();
            var existingStop = this.stopRepository.GetByJobDetails(job.PickListRef, job.PhAccount);

            if (existingStop == null)
            {
                this.logger.LogDebug(
                    $"Existing stop not found for picklist ({job.PickListRef}), account ({job.PhAccount})");
                this.eventLogger.TryWriteToEventLog(
                    EventSource.WellAdamXmlImport,
                    $"Existing stop not found for picklist ({job.PickListRef}), account ({job.PhAccount})",
                    7222);

                return;
            }

            this.mapper.Map(stop, existingStop);

            using (var transactionScope = new TransactionScope())
            {
                this.stopRepository.Update(existingStop);

                this.UpdateJobs(stop.Jobs, existingStop.Id);

                transactionScope.Complete();
            }
        }

        private void Delete(StopUpdate stopUpdate)
        {
            Stop stop = null;

            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    var job = stopUpdate.Jobs.First();

                    stop = this.stopRepository.GetByJobDetails(job.PickListRef, job.PhAccount);

                    this.stopRepository.DeleteStopByTransportOrderReference(stop.TransportOrderReference);

                    transactionScope.Complete();
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError($"Error on deletion of stop transport order reference ({stop.TransportOrderReference})", exception);
                this.eventLogger.TryWriteToEventLog(
                    EventSource.WellAdamXmlImport,
                    $"Error on deletion of stop transport order reference ({stop.TransportOrderReference})",
                    8332);
            }
        }

        private void UpdateJobs(IEnumerable<JobUpdate> jobs, int stopId)
        {
            foreach (var job in jobs)
            {
                var existingJob = this.jobRepository.GetJobByRefDetails(job.PhAccount, job.PickListRef, stopId);

                if (existingJob != null)
                {
                    this.mapper.Map(job, existingJob);

                    this.jobStatusService.SetIncompleteStatus(existingJob);

                    this.jobRepository.Update(existingJob);

                    this.UpdateJobDetails(job.JobDetails, existingJob.Id);
                }
                else
                {
                    var newJob = new Job();
                    this.mapper.Map(job, newJob);
                    newJob.StopId = stopId;
                    if (newJob.JobTypeCode == "DEL-DOC")
                    {
                        this.jobStatusService.SetInitialStatus(newJob);
                    }
                    else
                    {
                        this.jobStatusService.SetIncompleteStatus(newJob);
                    }
                    this.jobRepository.Save(newJob);
                }
            }
        }

        private void UpdateJobDetails(IEnumerable<JobDetailUpdate> jobDetails, int jobId)
        {
            foreach (var detail in jobDetails)
            {
                var existingJobDetail = this.jobDetailRepository.GetByJobLine(jobId, detail.LineNumber);

                if (existingJobDetail != null)
                {
                    this.mapper.Map(detail, existingJobDetail);

                    this.jobDetailRepository.Update(existingJobDetail);
                }
            }
        }

        private void InsertStops(StopUpdate stopInsert, RouteHeader header)
        {
            var job = stopInsert.Jobs.First();
            var existingStop = this.stopRepository.GetByJobDetails(job.PickListRef, job.PhAccount);

            if (existingStop != null)
            {
                this.logger.LogDebug($"Stop already exists for ({stopInsert.TransportOrderRef}) when doing adam insert to existing route header!");
                this.eventLogger.TryWriteToEventLog(
                    EventSource.WellAdamXmlImport,
                    $"Stop already exists for ({stopInsert.TransportOrderRef}) when doing adam insert to existing route header!",
                    3232);
                return;
            }

            using (var transactionScope = new TransactionScope())
            {
                var stop = new Stop
                {
                    RouteHeaderId = header.Id,
                    RouteHeaderCode = header.RouteNumber,
                    TransportOrderReference = stopInsert.TransportOrderRef
                };

                this.mapper.Map(stopInsert, stop);

                stop.PlannedStopNumber = stopInsert.DropNumber;

                this.stopRepository.Save(stop);

                this.InsertJobs(stopInsert.Jobs, stop.Id);

                transactionScope.Complete();
            }
        }

        private void InsertJobs(IEnumerable<JobUpdate> jobs, int stopId)
        {
            foreach (var update in jobs)
            {
                var job = new Job { StopId = stopId };

                this.jobStatusService.SetInitialStatus(job);

                this.mapper.Map(update, job);

                this.jobRepository.Save(job);

                this.InsertJobDetails(update.JobDetails, job.Id);
            }
        }

        private void InsertJobDetails(IEnumerable<JobDetailUpdate> jobDetails, int jobId)
        {
            foreach (var detail in jobDetails)
            {
                var jobDetail = new JobDetail { JobId = jobId, ShortsStatus = JobDetailStatus.UnRes };

                this.mapper.Map(detail, jobDetail);

                this.jobDetailRepository.Save(jobDetail);
            }
        }

        private static OrderActionIndicator GetOrderUpdateAction(string actionIndicator)
        {
            return string.IsNullOrWhiteSpace(actionIndicator) ? OrderActionIndicator.Update : StringExtensions.GetValueFromDescription<OrderActionIndicator>(actionIndicator);
        }
    }
}