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
        private readonly IOrderImportMapper mapper;
        private readonly IJobService jobStatusService;
        private readonly IPostImportRepository postImportRepository;

        public AdamUpdateService(
            ILogger logger,
            IEventLogger eventLogger,
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IJobRepository jobRepository,
            IJobDetailRepository jobDetailRepository,
            IOrderImportMapper mapper,
            IJobService jobStatusService,
            IPostImportRepository postImportRepository)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.mapper = mapper;
            this.jobStatusService = jobStatusService;
            this.postImportRepository = postImportRepository;
        }

        public void Update(RouteUpdates route)
        {
            //TODO: refactor for improvement. we may be able to put all insert/delete/update together and do it in async 
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
            var job = stop.Jobs.FirstOrDefault();

            if (job == null)
            {
                this.logger.LogDebug($"Stop with no jobs TransportOrderRef ({stop.TransportOrderRef})");
                return;
            }
            
            var branch = (int)Enum.Parse(typeof(Branches), stop.StartDepotCode, true);

            var existingStop = this.stopRepository.GetByJobDetails(job.PickListRef, job.PhAccount, branch);

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
                IList<int> updatedJobIds;
                this.UpdateJobs(stop.Jobs, existingStop.Id, out updatedJobIds);
                // updates Location/Activity/LineItem/Bag tables from imported data
                this.postImportRepository.PostImportUpdate(updatedJobIds);
                transactionScope.Complete();
            }
        }

        private void Delete(StopUpdate stopUpdate)
        {
            Stop stop = null;

            var branch = (int)Enum.Parse(typeof(Branches), stopUpdate.StartDepotCode, true);

            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    var job = stopUpdate.Jobs.First();

                    stop = this.stopRepository.GetByJobDetails(job.PickListRef, job.PhAccount, branch);

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

        private void UpdateJobs(IEnumerable<JobUpdate> jobs, int stopId, out IList<int> updatedJobIds)
        {
            updatedJobIds = new List<int>();
            foreach (var job in jobs)
            {
                var existingJob = this.jobRepository.GetJobByRefDetails(job.JobTypeCode, job.PhAccount, job.PickListRef, stopId);

                if (existingJob != null)
                {
                    this.mapper.Map(job, existingJob);

                    this.jobStatusService.SetIncompleteJobStatus(existingJob);

                    this.jobRepository.Update(existingJob);

                    this.UpdateJobDetails(job.JobDetails, existingJob.Id);
                    updatedJobIds.Add(existingJob.Id);
                }
                else
                {
                    var newJob = new Job();
                    this.mapper.Map(job, newJob);
                    newJob.StopId = stopId;
                    if (newJob.JobTypeCode == "DEL-DOC")
                    {
                        this.jobStatusService.SetInitialJobStatus(newJob);
                    }
                    else
                    {
                        this.jobStatusService.SetIncompleteJobStatus(newJob);
                    }
                    newJob.ResolutionStatus = ResolutionStatus.Imported;
                    this.jobRepository.Save(newJob);
                    foreach (var detail in job.JobDetails)
                    {
                        var newDetail = new JobDetail();
                        this.mapper.Map(detail, newDetail);
                        newDetail.JobId = newJob.Id;
                        newDetail.ShortsStatus = JobDetailStatus.UnRes;
                        this.jobDetailRepository.Save(newDetail);
                    }

                    this.jobRepository.SetJobResolutionStatus(newJob.Id, newJob.ResolutionStatus.Description);
                    updatedJobIds.Add(newJob.Id);
                }
            }
        }

        private void UpdateJobDetails(IEnumerable<JobDetailUpdate> jobDetails, int jobId)
        {
            var existingJobDetails = this.jobDetailRepository.GetByJobId(jobId).ToLookup(p => p.LineNumber);

            foreach (var detail in jobDetails)
            {
                var existingJobDetail = existingJobDetails[detail.LineNumber].FirstOrDefault();

                if (existingJobDetail != null)
                {
                    this.mapper.Map(detail, existingJobDetail);
                    this.jobDetailRepository.Update(existingJobDetail);
                }
                else
                {
                    // new jobdetail on Order file - tobacco bag jobdetail appears on Order but not on Route
                    var newJobDetail = new JobDetail();
                    this.mapper.Map(detail, newJobDetail);
                    newJobDetail.JobId = jobId;
                    newJobDetail.ShortsStatus = JobDetailStatus.Res;  // not sure why this is Resolved
                    this.jobDetailRepository.Save(newJobDetail);
                }
            }
        }

        private void InsertStops(StopUpdate stopInsert, RouteHeader header)
        {
            var job = stopInsert.Jobs.First();
            var existingStop = this.stopRepository.GetByJobDetails(job.PickListRef, job.PhAccount, header.RouteOwnerId);

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

                IList<int> insertedJobIds;
                this.InsertJobs(stopInsert.Jobs, stop.Id, out insertedJobIds);
                // updates Location/Activity/LineItem/Bag tables from imported data
                this.postImportRepository.PostImportUpdate(insertedJobIds);

                transactionScope.Complete();
            }
        }

        //TODO: refactor for improvement. All these records (jobs & jobdetails) can be send at one go to the store procedure
        private void InsertJobs(IEnumerable<JobUpdate> jobs, int stopId, out IList<int> insertedJobIds)
        {
            insertedJobIds = new List<int>();
            foreach (var update in jobs)
            {
                var job = new Job { StopId = stopId };

                this.jobStatusService.SetInitialJobStatus(job);
                job.ResolutionStatus = ResolutionStatus.Imported;

                this.mapper.Map(update, job);

                this.jobRepository.Save(job);
                this.jobRepository.SetJobResolutionStatus(job.Id, job.ResolutionStatus.Description);

                this.InsertJobDetails(update.JobDetails, job.Id);
                insertedJobIds.Add(job.Id);
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