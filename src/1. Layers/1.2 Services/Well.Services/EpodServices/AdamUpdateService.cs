namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
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

        private readonly IStopRepository stopRepository;

        private readonly IJobRepository jobRepository;

        private readonly IJobDetailRepository jobDetailRepository;

        private readonly IRouteMapper mapper;

        private const string CurrentUser = "AdamUpdate";

        public AdamUpdateService(
            ILogger logger,
            IEventLogger eventLogger,
            IStopRepository stopRepository,
            IJobRepository jobRepository,
            IJobDetailRepository jobDetailRepository,
            IRouteMapper mapper)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.mapper = mapper;

            this.stopRepository.CurrentUser = CurrentUser;
            this.jobRepository.CurrentUser = CurrentUser;
            this.jobDetailRepository.CurrentUser = CurrentUser;
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
            // TODO figure out how to match up the Identities
        }

        private void Update(StopUpdate stop)
        {
            var existingStop = this.stopRepository.GetByTransportOrderReference(stop.TransportOrderRef);

            if (existingStop == null)
            {
                this.logger.LogDebug(
                    $"Existing stop not found for transport order reference ({stop.TransportOrderRef})");
                this.eventLogger.TryWriteToEventLog(
                    EventSource.WellAdamXmlImport,
                    $"Existing stop not found for transport order reference ({stop.TransportOrderRef})",
                    7222);
            }
            else
            {
                this.mapper.Map(stop, existingStop);

                using (var transactionScope = new TransactionScope())
                {
                    this.stopRepository.Update(existingStop);

                    this.UpdateJobs(stop.Jobs, existingStop.Id);

                    transactionScope.Complete();
                }
            }
        }

        private void Delete(StopUpdate stop)
        {
            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    this.stopRepository.DeleteStopByTransportOrderReference(stop.TransportOrderRef);

                    transactionScope.Complete();
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError($"Error on deletion of stop transport order reference ({stop.TransportOrderRef})", exception);
                this.eventLogger.TryWriteToEventLog(
                    EventSource.WellAdamXmlImport,
                    $"Error on deletion of stop transport order reference ({stop.TransportOrderRef})",
                    8332);
            }
        }

        private void UpdateJobs(IEnumerable<JobUpdate> jobs, int stopId)
        {
            foreach (var job in jobs)
            {
                var existingJob = this.jobRepository.JobGetByRefDetails(job.PhAccount, job.PickListRef, stopId);

                if (existingJob != null)
                {
                    this.mapper.Map(job, existingJob);

                    this.jobRepository.Update(existingJob);

                    this.UpdateJobDetails(job.JobDetails, existingJob.Id);
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

        private static OrderActionIndicator GetOrderUpdateAction(string actionIndicator)
        {
            return string.IsNullOrWhiteSpace(actionIndicator) ? OrderActionIndicator.Update : StringExtensions.GetValueFromDescription<OrderActionIndicator>(actionIndicator);
        }
    }
}