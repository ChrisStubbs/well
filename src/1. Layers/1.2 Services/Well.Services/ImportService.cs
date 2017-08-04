namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.Extensions;
    using Repositories.Contracts;

    public class ImportService : IImportService
    {
        private readonly ILogger logger;
        private readonly IStopRepository stopRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IJobRepository jobRepository;
        private readonly IJobService jobService;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IJobDetailDamageRepository jobDetailDamageRepository;

        public ImportService(
            ILogger logger,
            IStopRepository stopRepository,
            IAccountRepository accountRepository,
            IJobRepository jobRepository,
            IJobService jobService,
            IJobDetailRepository jobDetailRepository,
            IJobDetailDamageRepository jobDetailDamageRepository
            )
        {
            this.logger = logger;
            this.stopRepository = stopRepository;
            this.accountRepository = accountRepository;
            this.jobRepository = jobRepository;
            this.jobService = jobService;
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
        }
        public virtual void ImportStops(RouteHeader fileRouteHeader)
        {
            var existingRouteStopsFromDb = stopRepository.GetStopByRouteHeaderId(fileRouteHeader.Id);

            IList<Stop> existingStopsBothSources = GetExistingStops(fileRouteHeader.Stops.Select(s => s.TransportOrderReference).Distinct().ToList());

            var savedStops = new List<Stop>();

            //loop throught all stops in the file
            foreach (var s in fileRouteHeader.Stops)
            {
                Stop fileStop = Mapper.Map<StopDTO, Stop>(s);

                var originalStop = FindOriginalStop(existingStopsBothSources, fileStop);

                fileStop.Id = originalStop?.Id ?? 0;

                //Is New
                if (fileStop.IsTransient())
                {
                    stopRepository.Save(fileStop);

                    fileStop.Jobs.ForEach(x => x.StopId = fileStop.Id);
                    fileStop.Account.StopId = fileStop.Id;
                    accountRepository.Save(fileStop.Account);
                    savedStops.Add(fileStop);
                }
                // Update Existing
                else if (!HasStopBeenCompleted(originalStop))
                {
                    fileStop.Previously = originalStop.SetPreviously(fileStop);
                    stopRepository.Update(fileStop);

                    fileStop.Jobs.ForEach(x => x.StopId = fileStop.Id);
                    fileStop.Account.StopId = fileStop.Id;
                    accountRepository.Update(fileStop.Account);
                    savedStops.Add(fileStop);
                }
                else
                {
                    var message = $"Ignoring Stop update. Stop is Complete  " +
                                  $"stop id ({originalStop.Id}) " +
                                  $"identifier ({originalStop.Identifier()}), " +
                                  $"route header Id ({originalStop.RouteHeaderId})";
                    logger.LogDebug(message);
                }
            }

            ImportJobs(fileRouteHeader.Id, fileRouteHeader.RouteOwnerId, savedStops.SelectMany(j => j.Jobs).ToList());

            //Delete Stops Not In File
            IEnumerable<Stop> stopsToBeDeleted = GetStopsToBeDeleted(existingRouteStopsFromDb, fileRouteHeader.Stops);

            foreach (var stopToBeDeleted in stopsToBeDeleted)
            {
                if (!HasStopBeenCompleted(stopToBeDeleted))
                {
                    var stopJobs = jobRepository.GetByStopId(stopToBeDeleted.Id);
                    if (stopJobs.All(CanWeUpdateJob))
                    {
                        stopToBeDeleted.DateDeleted = DateTime.Now;
                        stopToBeDeleted.DeletedByImport = true;
                        stopRepository.Update(stopToBeDeleted);
                    }
                }
            }
        }
        public void ImportJobs(int routeHeaderId, int branchId, IList<Job> jobs)
        {
            var existingStopJobsIds = jobRepository.GetJobIdsByRouteHeaderId(routeHeaderId);

            var existingJobsBothSources = GetExistingJobs(branchId, jobs);

            foreach (var job in jobs)
            {
                var originalJob = FindOriginalJob(existingJobsBothSources, job);

                if (originalJob == null)
                {
                    jobService.SetInitialJobStatus(job);
                    job.ResolutionStatus = ResolutionStatus.Imported;
                    jobRepository.Save(job);
                    jobRepository.SetJobResolutionStatus(job.Id, job.ResolutionStatus.Description);

                    job.JobDetails.ForEach(
                        x =>
                        {
                            x.JobId = job.Id;
                            x.ShortsStatus = JobDetailStatus.Res;
                            x.JobDetailReason = JobDetailReason.NotDefined;
                            x.JobDetailSource = JobDetailSource.NotDefined;
                        });

                    this.ImportJobDetails(job.JobDetails);
                }

                // Update Existings
                else if (CanWeUpdateJob(originalJob))
                {
                    originalJob.StopId = job.StopId;
                    jobRepository.Update(originalJob);
                }
                else
                {
                    var message = $"Ignoring Job update. Job is Complete  " +
                                  $"job id ({originalJob.Id}) " +
                                  $"identifier ({job.Identifier()}), " +
                                  $"branch id  ({branchId})";
                    logger.LogDebug(message);
                }
            }

            //Delete Jobs Not In File
            var jobsToBeDeleted = jobRepository.GetByIds(
                GetJobsIdsToBeDeleted(existingStopJobsIds, existingJobsBothSources.Select(x => x.Id))
            ).ToList();

            DeleteJobs(jobsToBeDeleted);

        }

        private IList<Stop> GetExistingStops(List<string> transportOrderReference)
        {
            var stopIds = stopRepository.GetStopByTransportOrderRefIncludingSoftDeleted(transportOrderReference).ToList();
            stopRepository.ReinstateStopSoftDeletedByImport(stopIds);
            return stopRepository.GetByIds(stopIds);
        }

        private bool HasStopBeenCompleted(Stop stop)
        {
            return stop.WellStatus == WellStatus.Complete;
        }

        private Stop FindOriginalStop(IList<Stop> existingStops, Stop stop)
        {
            return existingStops.FirstOrDefault(x => x.TransportOrderReference == stop.TransportOrderReference);
        }

        private IEnumerable<int> GetJobsIdsToBeDeleted(IEnumerable<int> existingStopJobIds, IEnumerable<int> existingJobIdsBothSources)
        {
            var existing = existingJobIdsBothSources.ToDictionary(k => k);
            return existingStopJobIds.Where(x => !existing.ContainsKey(x));
        }

        private void DeleteJobs(List<Job> jobsToBeDeleted)
        {
            foreach (var jobToDelete in jobsToBeDeleted)
            {
                if (jobToDelete.IsDocumentDelivery() || CanWeUpdateJob(jobToDelete))
                {
                    this.jobRepository.CascadeSoftDeleteJobs(new[] { jobToDelete.Id }, true);
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

        private IList<Job> GetExistingJobs(int branchId, IList<Job> jobs)
        {
            var existing = jobRepository.
                GetExistingJobsIdsIncludingSoftDeleted(branchId,
                    jobs.Where(x => !x.IsDocumentDelivery())
                ).ToList();

            jobRepository.ReinstateJobsSoftDeletedByImport(existing);

            return jobRepository.GetByIds(existing).ToList();
        }

     
        private void ImportJobDetails(IEnumerable<JobDetail> jobDetails)
        {
            foreach (var detail in jobDetails)
            {
                this.jobDetailRepository.Save(detail);

                detail.JobDetailDamages.ForEach(
                    x =>
                    {
                        x.JobDetailId = detail.Id;
                        x.DamageStatus = x.Qty == 0 ? JobDetailStatus.Res : JobDetailStatus.UnRes;
                    });

                this.ImportJobDetailDamages(detail.JobDetailDamages);
            }
        }

        private void ImportJobDetailDamages(IEnumerable<JobDetailDamage> damages)
        {
            foreach (var damage in damages)
            {
                this.jobDetailDamageRepository.Save(damage);
            }
        }

        private bool CanWeUpdateJob(Job job)
        {
            switch (job.WellStatus)
            {
                case WellStatus.Complete:
                    return false;
                case WellStatus.Bypassed:
                    return true;
                default:
                    return true;
            }

        }

        private Job FindOriginalJob(IList<Job> existingJobs, Job job)
        {
            return existingJobs.FirstOrDefault(x =>
                x.Identifier() == job.Identifier()
            );
        }
    }
}