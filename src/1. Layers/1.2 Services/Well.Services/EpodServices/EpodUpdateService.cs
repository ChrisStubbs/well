﻿namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class EpodUpdateService : IEpodUpdateService
    {
        private readonly ILogger logger;
        private readonly IEventLogger eventLogger;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IStopRepository stopRepository;
        private readonly IJobRepository jobRepository;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IJobDetailDamageRepository jobDetailDamageRepository;

        private readonly IRouteMapper mapper;
        private const string UpdatedBy = "EpodUpdate";

        public EpodUpdateService(
            ILogger logger,
            IEventLogger eventLogger,
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IJobRepository jobRepository,
            IJobDetailRepository jobDetailRepository,
            IJobDetailDamageRepository jobDetailDamageRepository,
            IRouteMapper mapper)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
            this.mapper = mapper;

            this.routeHeaderRepository.CurrentUser = UpdatedBy;
            this.stopRepository.CurrentUser = UpdatedBy;
            this.jobRepository.CurrentUser = UpdatedBy;
            this.jobDetailRepository.CurrentUser = UpdatedBy;
            this.jobDetailDamageRepository.CurrentUser = UpdatedBy;
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

                this.mapper.Map(job, existingJob);

                this.jobRepository.Update(existingJob);

                this.UpdateJobDetails(job.JobDetails, existingJob.Id, string.IsNullOrWhiteSpace(existingJob.InvoiceNumber));
            }
        }

        private void UpdateJobDetails(IEnumerable<JobDetail> jobDetails, int jobId, bool invoiceOutstanding)
        {
            foreach (var detail in jobDetails)
            {
                var existingJobDetail = this.jobDetailRepository.GetByJobLine(jobId, detail.LineNumber);

                if (existingJobDetail == null)
                {
                    this.logger.LogDebug($"Ecisting job detail not found for job id ({jobId}), line number ({detail.LineNumber})");
                    continue;
                }

                this.mapper.Map(detail, existingJobDetail);

                // TODO might need to set resolved unresolved status here and add in sub outer values

                if (invoiceOutstanding)
                    existingJobDetail.JobDetailStatusId = (int)JobDetailStatus.AwtInvNum;
                
                this.jobDetailRepository.Update(existingJobDetail);

                this.UpdateJobDamages(detail.JobDetailDamages, existingJobDetail.Id);
            }
        }

        private void UpdateJobDamages(IEnumerable<JobDetailDamage> damages, int jobDetailId)
        {
            damages.ToList().ForEach(x => x.JobDetailId = jobDetailId);

            foreach (var damage in damages)
            {
                this.jobDetailDamageRepository.Save(damage);
            }
        }
    }
}