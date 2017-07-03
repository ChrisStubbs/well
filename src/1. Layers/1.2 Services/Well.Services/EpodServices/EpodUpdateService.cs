namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;

    using Domain.ValueObjects;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;
    using static PH.Well.Domain.Mappers.AutoMapperConfig;

    public class EpodUpdateService : IEpodUpdateService
    {
        private readonly ILogger logger;
        private readonly IEventLogger eventLogger;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IStopRepository stopRepository;
        private readonly IJobRepository jobRepository;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IJobDetailDamageRepository jobDetailDamageRepository;
        private readonly IExceptionEventRepository exceptionEventRepository;
        private readonly IRouteMapper mapper;
        private readonly IJobService jobService;
        private readonly IPostImportRepository postImportRepository;
        private readonly IJobResolutionStatus jobResolutionStatus;
        private readonly IDateThresholdService _dateThresholdService;
        private const int EventLogErrorId = 9682;
        private const int ProcessTypeForGrn = 1;

        public EpodUpdateService(
            ILogger logger,
            IEventLogger eventLogger,
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IJobRepository jobRepository,
            IJobDetailRepository jobDetailRepository,
            IJobDetailDamageRepository jobDetailDamageRepository,
            IExceptionEventRepository exceptionEventRepository,
            IRouteMapper mapper,
            IJobService jobService,
            IPostImportRepository postImportRepository,
            IJobResolutionStatus jobResolutionStatus,
            IDateThresholdService dateThresholdService)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
            this.exceptionEventRepository = exceptionEventRepository;
            this.mapper = mapper;
            this.jobService = jobService;
            this.postImportRepository = postImportRepository;
            this.jobResolutionStatus = jobResolutionStatus;
            
            _dateThresholdService = dateThresholdService;
        }

        public void Update(RouteDelivery route, string fileName)
        {
            var updatedJobIds = new List<int>();
            foreach (var header in route.RouteHeaders)
            {
                int branchId;
                if (header.TryParseBranchIdFromRouteNumber(out branchId))
                {
                    var existingHeader = this.routeHeaderRepository.GetRouteHeaderByRoute(
                        branchId,
                        header.RouteNumber.Substring(2),
                        header.RouteDate);

                    if (existingHeader == null)
                    {
                        var message = $"RouteDelivery Ignored could not find matching RouteHeader," +
                                      $"Branch: {branchId} " +
                                      $"RouteNumber: {header.RouteNumber.Substring(2)} " +
                                      $"RouteDate: {header.RouteDate} " +
                                      $"FileName: {fileName}";
                        logger.LogDebug(message);
                        eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, message, EventLogErrorId);

                        //this.adamImportService.ImportRouteHeader(header, route.RouteId);
                        continue;
                    }

                    this.mapper.Map(header, existingHeader);

                    this.routeHeaderRepository.Update(existingHeader);

                    var jobIdsForHeader = this.UpdateStops(header, branchId);

                    updatedJobIds.AddRange(jobIdsForHeader);
                }
                else
                {
                    var message = $" Route Number Depot Indicator is not an int... Route Number Depot passed in from from transend is ({header.RouteNumber}) file {fileName}";
                    this.logger.LogDebug(message);
                    this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, message, EventLogErrorId);
                }

            }

            // updates Location/Activity/LineItem/Bag tables from imported data
            this.postImportRepository.PostImportUpdate();
            // updates tobacco lines from tobacco bag data
            this.postImportRepository.PostTranSendImportForTobacco();

            // updates LineItemActions imported data
            this.postImportRepository.PostTranSendImport();
            //updates Jobs with data for shorts to be advised
            this.postImportRepository.PostTranSendImportShortsTba(updatedJobIds);


            // update JobResolutionStatus for jobs with LineItemActions
            if (updatedJobIds.Count != 0)
            {
                var idsForJobsWithActions = jobRepository.GetJobsWithLineItemActions(updatedJobIds);
                var updatedJobs = jobService.PopulateLineItemsAndRoute(jobRepository.GetByIds(idsForJobsWithActions));

                foreach (var job in updatedJobs)
                {
                    var status = this.jobResolutionStatus.GetNextResolutionStatus(job);
                    if (status != ResolutionStatus.Invalid)
                    {
                        job.ResolutionStatus = status;
                    }

                    this.jobRepository.Update(job);
                    this.jobRepository.SetJobResolutionStatus(job.Id, job.ResolutionStatus.Description);
                }
            }
        }

        private List<int> UpdateStops(RouteHeader routeHeader, int branchId)
        {
            var updatedJobIds
                = new List<int>();
            foreach (var stop in routeHeader.Stops)
            {
                try
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        var job = stop.Jobs.First();
                        var existingStop = this.stopRepository.GetByJobDetails(job.PickListRef, job.PhAccount);

                        if (existingStop == null)
                        {
                            this.logger.LogDebug(
                                $"Existing stop not found with transport order reference {stop.TransportOrderReference}");
                            this.eventLogger.TryWriteToEventLog(
                                EventSource.WellAdamXmlImport,
                                $"Existing stop not found with transport order reference {stop.TransportOrderReference}",
                                7666);

                            continue;
                        }

                        this.mapper.Map(stop, existingStop);

                        this.stopRepository.Update(existingStop);

                        var updates = this.UpdateJobs(stop.Jobs, existingStop.Id, branchId, routeHeader.RouteDate.Value);
                        updatedJobIds.AddRange(updates);

                        transactionScope.Complete();
                    }
                }
                catch (Exception exception)
                {
                    this.logger.LogError(
                        $"Stop has an error on Epod update! Stop Id ({stop.Id}), Transport order reference ({stop.TransportOrderReference})",
                        exception);
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"Stop has an error on Epod update! Stop Id ({stop.Id}), Transport order reference ({stop.TransportOrderReference})",
                        9859);
                }
            }

            return updatedJobIds;
        }

        private List<int> UpdateJobs(IEnumerable<JobDTO> jobs, int stopId, int branchId,DateTime routeDate)
        {
            var updatedJobIds = new List<int>();

            foreach (var job in jobs)
            {
                var existingJob = this.jobRepository.GetJobByRefDetails(
                    job.JobTypeCodeTransend,
                    job.PhAccount,
                    job.PickListRef,
                    stopId);

                if (existingJob == null)
                {
                    continue;
                }

                updatedJobIds.Add(existingJob.Id);

                this.mapper.Map(job, existingJob);

                this.jobService.DetermineStatus(existingJob, branchId);
                existingJob.ResolutionStatus = ResolutionStatus.DriverCompleted;

                if (!string.IsNullOrWhiteSpace(job.GrnNumber) && existingJob.GrnProcessType == ProcessTypeForGrn)
                {
                    var grnEvent = new GrnEvent { Id = existingJob.Id, BranchId = branchId };

                    this.exceptionEventRepository.InsertGrnEvent(grnEvent,
                        _dateThresholdService.EarliestSubmitDate(routeDate, branchId));
                }

                this.UpdateJobDetails(
                    job.JobDetails,
                    existingJob.Id);

                var pod = existingJob.ProofOfDelivery.GetValueOrDefault();

                if ((pod == (int)ProofOfDelivery.CocaCola || pod == (int)ProofOfDelivery.Lucozade ) && existingJob.JobStatus != JobStatus.CompletedOnPaper)
                {
                    var podEvent = new PodEvent
                    {
                        BranchId = branchId,
                        Id = existingJob.Id
                    };
                    this.exceptionEventRepository.InsertPodEvent(podEvent);
                }

                this.jobRepository.Update(existingJob);
                this.jobRepository.SetJobResolutionStatus(existingJob.Id, existingJob.ResolutionStatus.Description);
            }
            return updatedJobIds;
        }

        private void UpdateJobDetails(IEnumerable<JobDetailDTO> jobDetails, int jobId)
        {
            foreach (var detail in jobDetails)
            {
                var existingJobDetail = this.jobDetailRepository.GetByJobLine(jobId, detail.LineNumber);

                detail.ShortsStatus = detail.ShortQty == 0 ? JobDetailStatus.Res : JobDetailStatus.UnRes;

                if (existingJobDetail == null)
                {
                    detail.JobId = jobId;

                    this.jobDetailRepository.Save(Mapper.Map<JobDetailDTO, JobDetail>(detail));

                    continue;
                }

                this.mapper.Map(detail, existingJobDetail);

                existingJobDetail.SkuGoodsValue = detail.SkuGoodsValue;

                if (existingJobDetail.ShortQty > 0)
                {
                    existingJobDetail.JobDetailReason = JobDetailReason.NotDefined;
                    existingJobDetail.JobDetailSource = JobDetailSource.NotDefined;

                }

                this.jobDetailRepository.Update(existingJobDetail);

                this.UpdateJobDamages(detail.JobDetailDamages, existingJobDetail.Id);
            }
        }

        private void UpdateJobDamages(IEnumerable<JobDetailDamageDTO> damages, int jobDetailId)
        {
            damages.ToList().ForEach(x => x.JobDetailId = jobDetailId);

            foreach (var damage in damages)
            {
                if (!string.IsNullOrWhiteSpace(damage.Reason.Description) && (
                    damage.Reason.Description.ToLower().Contains("short") || 
                    damage.Reason.Description.ToLower().Contains("product not available")))
                {
                    continue;
                }

                damage.PdaReasonDescription = string.IsNullOrWhiteSpace(damage.Reason.Description) ? "Not defined" : damage.Reason.Description;

                damage.JobDetailReason = JobDetailReason.NotDefined;
                damage.DamageStatus = damage.Qty == 0 ? JobDetailStatus.Res : JobDetailStatus.UnRes;
              
                this.jobDetailDamageRepository.Save(Mapper.Map<JobDetailDamageDTO, JobDetailDamage>(damage));
            }
        }
    }
}