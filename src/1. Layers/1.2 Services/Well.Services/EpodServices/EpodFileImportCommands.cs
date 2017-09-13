namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;
    using static PH.Well.Domain.Mappers.AutoMapperConfig;

    public class EpodFileImportCommands : IEpodFileImportCommands
    {
        private readonly ILogger logger;
        private readonly IJobRepository jobRepository;
        private readonly IEpodImportMapper epodImportMapper;
        private readonly IJobService jobService;
        private readonly IExceptionEventRepository exceptionEventRepository;
        private readonly IDateThresholdService dateThresholdService;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IJobDetailDamageRepository jobDetailDamageRepository;
        private readonly IPostImportRepository postImportRepository;
        private readonly IPodService podService;

        public EpodFileImportCommands(
            ILogger logger,
            IJobRepository jobRepository,
            IEpodImportMapper epodImportMapper,
            IJobService jobService,
            IExceptionEventRepository exceptionEventRepository,
            IDateThresholdService dateThresholdService,
            IJobDetailRepository jobDetailRepository,
            IJobDetailDamageRepository jobDetailDamageRepository,
            IPostImportRepository postImportRepository,
            IPodService podService)
        {
            this.logger = logger;
            this.jobRepository = jobRepository;
            this.epodImportMapper = epodImportMapper;
            this.jobService = jobService;
            this.exceptionEventRepository = exceptionEventRepository;
            this.dateThresholdService = dateThresholdService;
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
            this.postImportRepository = postImportRepository;
            this.podService = podService;
        }

        public void AfterJobCreation(Job fileJob, Job existingJob, RouteHeader routeHeader)
        {
            UpdateExistingJobWithEvents(fileJob, existingJob, routeHeader);
        }

        public void UpdateWithoutEvents(Job existingJob, int branchId, DateTime routeDate)
        {
            UpdateExistingJob(existingJob, existingJob, branchId, routeDate, false, false);
        }

        public void UpdateWithEvents(Job existingJob, int branchId, DateTime routeDate)
        {
            UpdateExistingJob(existingJob, existingJob, branchId, routeDate, true, false);
        }

        public void UpdateExistingJob(Job fileJob, Job existingJob, RouteHeader routeHeader)
        {
            UpdateExistingJobWithEvents(fileJob, existingJob, routeHeader);
        }

        private void UpdateExistingJobWithEvents(Job fileJob, Job existingJob, RouteHeader routeHeader)
        {
            existingJob.ResolutionStatus = ResolutionStatus.DriverCompleted;
            UpdateExistingJob(fileJob, existingJob, routeHeader.RouteOwnerId, routeHeader.RouteDate.Value, true, false);
        }

        //TODO: Need more tests around this
        public virtual void UpdateExistingJob(Job fileJob, Job existingJob, int branchId, DateTime routeDate,bool createEvents, bool includePodBypass)
        {
            this.epodImportMapper.MapJob(fileJob, existingJob);

            this.jobService.DetermineStatus(existingJob, branchId);

            if (createEvents && existingJob.IsGrnNumberRequired &&
                !exceptionEventRepository.GrnEventCreatedForJob(existingJob.Id.ToString()))
            {
                var grnEvent = new GrnEvent {Id = existingJob.Id, BranchId = branchId};

                this.exceptionEventRepository.InsertGrnEvent(grnEvent,
                    dateThresholdService.GracePeriodEnd(routeDate, branchId, existingJob.GetRoyaltyCode()),
                    existingJob.Id.ToString());
            }

            this.UpdateJobDetails(
                fileJob.JobDetails,
                existingJob.Id);

            // do not create pod event for bypass, unless actioned by cs
            if (createEvents &&
                existingJob.IsProofOfDelivery &&
                existingJob.JobStatus != JobStatus.CompletedOnPaper &&
                (existingJob.JobStatus != JobStatus.Bypassed || IncludeBypass(includePodBypass, existingJob.JobStatus)))
            {
                this.podService.CreatePodEvent(existingJob, branchId, routeDate);
            }


            // GRN event shouldn probably be created during epod update
            //TODO: This needs to re-instated
            //ProcessGlobalUplift(fileJob, existingJob, branchId, createEvents);

            this.jobRepository.Update(existingJob);
            this.jobRepository.SetJobResolutionStatus(existingJob.Id, existingJob.ResolutionStatus.Description);
        }

        private void ProcessGlobalUplift(Job fileJob, Job existingJob, int branchId, bool createEvents)
        {
            if (createEvents && existingJob.JobTypeCode == "UPL-GLO" && existingJob.JobStatus != JobStatus.Bypassed &&
                            !exceptionEventRepository.GlobalUpliftEventCreatedForJob(existingJob.Id.ToString()))
            {
                var globalJobDetail = fileJob.JobDetails.FirstOrDefault();

                int csfNumber = 0;

                if (!Int32.TryParse(existingJob.PickListRef, out csfNumber))
                {
                    //maybe picklist reference, not invoice number
                    //log error?
                    this.logger.LogError(
                        $"(No csf number for global uplift ({existingJob.Id}))");

                    csfNumber = 0;
                }

                int productCode = 0;

                if (!Int32.TryParse(globalJobDetail.PhProductCode, out productCode))
                {
                    //log error?
                    this.logger.LogError(
                        $"(No product code for global uplift ({existingJob.Id}))");
                    productCode = 0;
                }

                var globalUpliftEvent = new GlobalUpliftEvent
                {
                    BranchId = branchId,
                    Id = existingJob.Id,
                    AccountNumber = existingJob.PhAccount,
                    CreditReasonCode = "24",
                    CsfNumber = csfNumber,
                    ProductCode = productCode,
                    Quantity = globalJobDetail.DeliveredQty,
                    WriteLine = true,
                    WriteHeader = true
                };

                this.exceptionEventRepository.InsertGlobalUpliftEvent(globalUpliftEvent, existingJob.Id.ToString());
                // insert a report event?
            }
        }

        private void UpdateJobDetails(IEnumerable<JobDetail> jobDetails, int jobId)
        {
            var existingJobDetails = this.jobDetailRepository.GetByJobId(jobId).ToLookup(p => p.LineNumber);

            foreach (var detail in jobDetails)
            {
                var existingJobDetail = existingJobDetails[detail.LineNumber].FirstOrDefault();

                detail.ShortsStatus = detail.ShortQty == 0 ? JobDetailStatus.Res : JobDetailStatus.UnRes;

                if (existingJobDetail == null)
                {
                    detail.JobId = jobId;

                    jobDetailRepository.Save(detail);

                    continue;
                }

                epodImportMapper.MapJobDetail(detail, existingJobDetail);
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

        private void UpdateJobDamages(IEnumerable<JobDetailDamage> damages, int jobDetailId)
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

                damage.PdaReasonDescription = string.IsNullOrWhiteSpace(damage.Reason.Description)
                    ? "Not defined"
                    : damage.Reason.Description;

                damage.JobDetailReason = JobDetailReason.NotDefined;
                damage.DamageStatus = damage.Qty == 0 ? JobDetailStatus.Res : JobDetailStatus.UnRes;

                this.jobDetailDamageRepository.Save(damage);
            }
        }

        public void PostJobImport(IList<int> jobIds)
        {
            // updates Location/Activity/LineItem/Bag tables from imported data
            this.postImportRepository.PostImportUpdate(jobIds);
            //On Transend Import
            RunPostInvoicedProcessing(jobIds);
        }

        public IList<Job> GetJobsToBeDeleted(IList<JobStop> existingRouteJobIdAndStopId, IList<Job> existingJobsBothSources, IList<Stop> completedStops)
        {
            // For Epod files only delete the jobs for the stops
            var currentStops = existingJobsBothSources.Select(s => s.StopId).Distinct();
            var existingJobIdsForStops = existingRouteJobIdAndStopId.Where(x => currentStops.Contains(x.StopId));
            var jobIdsToBeDeleted = GetJobsIdsToBeDeleted(existingJobIdsForStops.Select(x => x.JobId),
                existingJobsBothSources.Select(x => x.Id));

            return jobRepository.GetByIds(jobIdsToBeDeleted).Where(j => !completedStops.Select(s => s.Id).Contains(j.StopId)).ToList();
        }

        private IEnumerable<int> GetJobsIdsToBeDeleted(IEnumerable<int> existingStopJobIds,
            IEnumerable<int> existingJobIdsBothSources)
        {
            var existing = existingJobIdsBothSources.ToDictionary(k => k);
            return existingStopJobIds.Where(x => !existing.ContainsKey(x));
        }

        public virtual IList<Job> RunPostInvoicedProcessing(IList<int> updatedJobIds)
        {
            var jobs = new List<Job>();
            if (updatedJobIds == null)
            {
                throw new ArgumentNullException(nameof(updatedJobIds));
            }

            // update JobResolutionStatus for jobs with LineItemActions
            if (updatedJobIds.Count != 0)
            {
                // updates tobacco lines from tobacco bag data
                this.postImportRepository.PostTranSendImportForTobacco(updatedJobIds);
                // updates LineItemActions imported data
                this.postImportRepository.PostTranSendImport(updatedJobIds);
                //updates Jobs with data for shorts to be advised
                this.postImportRepository.PostTranSendImportShortsTba(updatedJobIds);

                var idsForJobsWithActions = jobRepository.GetJobsWithLineItemActions(updatedJobIds);
                var updatedJobs = jobService.PopulateLineItemsAndRoute(jobRepository.GetByIds(idsForJobsWithActions));

                foreach (var job in updatedJobs)
                {
                    var status = this.jobService.GetNextResolutionStatus(job);
                    if (status != ResolutionStatus.Invalid)
                    {
                        job.ResolutionStatus = status;
                    }

                    this.jobRepository.Update(job);
                    this.jobRepository.SetJobResolutionStatus(job.Id, job.ResolutionStatus.Description);

                    jobs.Add(job);
                }

                // Compute well status for jobs
                foreach (var jobId in updatedJobIds)
                {
                    jobService.ComputeWellStatus(jobId);
                }
            }

            return jobs;
        }

        private bool IncludeBypass(bool includePodBypass, JobStatus status)
        {
            if (status == JobStatus.Bypassed)
            {
                if (includePodBypass == true)
                {
                    return true;
                }
            }

            return false;
        }


    }
}
    
