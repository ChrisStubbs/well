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

        public AdamFileImportCommands(
            IJobRepository jobRepository,
            IPostImportRepository postImportRepository,
            IStopRepository stopRepository,
            IJobDetailRepository jobDetailRepository,
            IAdamImportMapper importMapper)
        {
            this.jobRepository = jobRepository;
            this.postImportRepository = postImportRepository;
            this.stopRepository = stopRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.importMapper = importMapper;
        }
      
        public void UpdateExistingJob(Job fileJob, Job existingJob, RouteHeader routeHeader)
        {
            importMapper.MapJob(fileJob,existingJob);
            jobRepository.Update(existingJob);
            UpdateJobDetails(fileJob.JobDetails, existingJob.Id);
        }

        public void PostJobImport(IList<int> jobIds)
        {
            this.postImportRepository.PostImportUpdate(jobIds);
        }

        public IList<Job> GetJobsToBeDeleted(IList<JobStop> existingRouteJobIdAndStopId, IList<Job> existingJobsBothSources)
        {
            //Adam File will contain all jobs for all stops so delete anything that is not in the latest file
            var jobIdsToDelete = GetJobsIdsToBeDeleted(existingRouteJobIdAndStopId.Select(x => x.JobId), existingJobsBothSources.Select(x => x.Id));

            return jobRepository.GetByIds(jobIdsToDelete).ToList();
            
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
                    if (stopJobs.All(x=> x.CanWeUpdateJobOnImport()))
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
            foreach (var detail in jobDetails)
            {
                var existingJobDetail = this.jobDetailRepository.GetByJobLine(jobId, detail.LineNumber);

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