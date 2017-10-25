namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.Extensions;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class AdamFileImportCommands : IAdamFileImportCommands
    {
        private readonly IJobRepository jobRepository;
        private readonly IPostImportRepository postImportRepository;
        private readonly IStopRepository stopRepository;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IAdamImportMapper importMapper;
        private readonly ILineItemActionRepository lineItemActionRepository;
        private readonly IJobService jobService;

        public AdamFileImportCommands(
            IJobRepository jobRepository,
            IPostImportRepository postImportRepository,
            IStopRepository stopRepository,
            IJobDetailRepository jobDetailRepository,
            IAdamImportMapper importMapper,
            ILineItemActionRepository lineItemActionRepository,
            IJobService jobService)
        {
            this.jobRepository = jobRepository;
            this.postImportRepository = postImportRepository;
            this.stopRepository = stopRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.importMapper = importMapper;
            this.lineItemActionRepository = lineItemActionRepository;
            this.jobService = jobService;
        }

        public void UpdateExistingJob(Job fileJob, Job existingJob, RouteHeader routeHeader, bool isJobReplanned)
        {
            importMapper.MapJob(fileJob, existingJob);

            if (isJobReplanned)
            {
                existingJob.JobStatus = JobStatus.Replanned;
                existingJob.WellStatus = existingJob.JobStatus.ToWellStatus();
                lineItemActionRepository.DeleteAllLineItemActionsForJob(existingJob.Id);
                existingJob.ResolutionStatus = ResolutionStatus.Imported;
            }

            jobRepository.UpdateReinstateJob(existingJob);
            UpdateJobDetails(fileJob.JobDetails, existingJob.Id);
        }

        public void UpdateExistingJobFromReinstateJob(Job fileJob, ReinstateJob existingJob, RouteHeader routeHeader, bool isJobReplanned)
        {
            this.UpdateExistingJob(fileJob,
                new Job
                {
                    Id = existingJob.JobId,
                    GrnNumber = existingJob.GrnNumber,
                    RoyaltyCode = existingJob.RoyaltyCode,
                    ResolutionStatus = ResolutionStatus.DriverCompleted,
                    StopId = existingJob.StopId,
                    PhAccount = existingJob.PhAccount,
                    PickListRef = existingJob.PickListRef,
                    JobTypeCode = existingJob.JobTypeCode,
                    WellStatus = existingJob.WellStatus,
                    JobStatus = existingJob.JobStatus,

                    Sequence = existingJob.Sequence,
                    CustomerRef = existingJob.CustomerRef,
                    OrdOuters = existingJob.OrdOuters,
                    InvOuters = existingJob.InvOuters,
                    Cod = existingJob.Cod,
                    JobByPassReason = existingJob.Reason,
                    OuterCount = existingJob.OuterCount,
                    TotalOutersOver = existingJob.TotalOutersOver,
                    DetailOutersOver = existingJob.DetailOutersOver,
                    Picked = existingJob.Picked,
                    TotalOutersShort = existingJob.TotalOutersShort,
                    PerformanceStatus = (PerformanceStatus)existingJob.PerformanceStatusId,
                    ProofOfDelivery = existingJob.ProofOfDelivery,
                    InvoiceNumber = existingJob.InvoiceNumber
                }, routeHeader, isJobReplanned);
        }

        public void PostJobImport(IList<int> jobIds)
        {
            this.postImportRepository.PostImportUpdate(jobIds);
            // calculate
        }

        public IList<Job> GetJobsToBeDeleted(IList<JobStop> existingRouteJobIdAndStopId, IList<Tuple<int, int>> existingJobIdsBothSources, IList<Stop> completedStops)
        {
            //Adam File will contain all jobs for all stops so delete anything that is not in the latest file
            var jobIdsToDelete = GetJobsIdsToBeDeleted(
                existingRouteJobIdAndStopId.Select(x => x.JobId), 
                existingJobIdsBothSources.Select(p => p.Item1).ToList());

            return jobRepository.GetByIds(jobIdsToDelete).Where(j => !completedStops.Select(s => s.Id).Contains(j.StopId)).ToList();
        }

        private IEnumerable<int> GetJobsIdsToBeDeleted(IEnumerable<int> existingRouteJobIds, IEnumerable<int> existingJobIdsBothSources)
        {
            var existing = existingJobIdsBothSources.ToDictionary(k => k);
            return existingRouteJobIds.Where(x => !existing.ContainsKey(x));
        }

        public void DeleteStopsNotInFile(IEnumerable<Stop> existingRouteStopsFromDb, List<StopDTO> stops)
        {
            IEnumerable<Stop> stopsToBeDeleted = GetStopsToBeDeleted(existingRouteStopsFromDb, stops);

            foreach (var stopToBeDeleted in stopsToBeDeleted)
            {
                if (!stopToBeDeleted.HasStopBeenCompleted())
                {
                    var stopJobs = jobRepository.GetByStopId(stopToBeDeleted.Id);
                    if (stopJobs.All(x => x.CanWeUpdateJobOnImport()))
                    {
                        stopToBeDeleted.DateDeleted = DateTime.Now;
                        stopToBeDeleted.DeletedByImport = true;
                        stopRepository.Update(stopToBeDeleted);
                    }
                }
            }
        }

        private IEnumerable<Stop> GetStopsToBeDeleted(IEnumerable<Stop> existRouteStops, List<StopDTO> fileStops)
        {
            var fileTransportOrderRef = fileStops
                .Select(s => s.TransportOrderReference)
                .Distinct()
                .ToDictionary(k => k, v => v, StringComparer.OrdinalIgnoreCase);

            return existRouteStops
                .Where(x => !fileTransportOrderRef.ContainsKey(x.TransportOrderReference));
        }

        private void UpdateJobDetails(IEnumerable<JobDetail> jobDetails, int jobId)
        {
            var existingJobDetails = this.jobDetailRepository.GetByJobId(jobId).ToLookup(p => p.LineNumber);
            foreach (var detail in jobDetails)
            {
                var existingJobDetail = existingJobDetails[detail.LineNumber].FirstOrDefault();

                if (existingJobDetail != null)
                {
                    importMapper.MapJobDetail(detail, existingJobDetail);

                    this.jobDetailRepository.Update(existingJobDetail);
                }
                else
                {
                    // new jobdetail on Order file - tobacco bag jobdetail appears on Order but not on Route
                    var newJobDetail = new JobDetail();
                    importMapper.MapJobDetail(detail, newJobDetail);
                    newJobDetail.JobId = jobId;
                    newJobDetail.ShortsStatus = JobDetailStatus.Res;  // not sure why this is Resolved
                    this.jobDetailRepository.Save(newJobDetail);
                }
            }
        }
    }
}