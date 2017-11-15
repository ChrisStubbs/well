namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Contracts;
    using Domain.Enums;
    using Domain.Extensions;
    using Domain.Mappers;
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
        private readonly IStopService stopService;

        public ImportService(
            ILogger logger,
            IStopRepository stopRepository,
            IAccountRepository accountRepository,
            IJobRepository jobRepository,
            IJobService jobService,
            IJobDetailRepository jobDetailRepository,
            IJobDetailDamageRepository jobDetailDamageRepository,
            IStopService stopService
            )
        {
            this.logger = logger;
            this.stopRepository = stopRepository;
            this.accountRepository = accountRepository;
            this.jobRepository = jobRepository;
            this.jobService = jobService;
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
            this.stopService = stopService;
        }

        //TODO: Unit Tests
        public virtual void ImportStops(RouteHeader fileRouteHeader, IImportMapper importMapper, IImportCommands importCommands)
        {
            AddHeaderInformationToStops(fileRouteHeader);

            var existingRouteStopsFromDb = stopRepository.GetStopByRouteHeaderId(fileRouteHeader.Id);

            var existingStopsBothSources = GetExistingStops(fileRouteHeader.Stops.Select(s => s.TransportOrderReference).Distinct().ToList())
                .ToDictionary(k => k.TransportOrderReference, StringComparer.CurrentCultureIgnoreCase);

            var stopImportStatuses = new List<StopImportStatus>();

            //loop through all stops in the file
            foreach (var s in fileRouteHeader.Stops)
            {
                Stop fileStop = AutoMapperConfig.Mapper.Map<StopDTO, Stop>(s);

                var originalStop = existingStopsBothSources.ContainsKey(fileStop.TransportOrderReference)
                    ? existingStopsBothSources[fileStop.TransportOrderReference] : (Stop)null;

                //Is New
                if (originalStop == null)
                {
                    stopRepository.Save(fileStop);
                    Parallel.ForEach(fileStop.Jobs, x => x.StopId = fileStop.Id);
                    fileStop.Account.StopId = fileStop.Id;
                    accountRepository.Save(fileStop.Account);
                    stopImportStatuses.Add(new StopImportStatus(fileStop, StopImportStatus.Status.New));

                    s.Id = fileStop.Id;
                    Parallel.ForEach(s.Jobs, x => x.StopId = fileStop.Id);
                }
                // Update Existing
                else if (IsEpodImport(importCommands) || !originalStop.HasStopBeenCompleted())
                {
                    var originalStopIdentifiers = originalStop.CloneStopIdentifiers();
                    originalStop.Previously = originalStop.GetPreviously(fileStop);
                    importMapper.MapStop(fileStop, originalStop);
                    stopRepository.Update(originalStop);
                    fileStop.Id = originalStop.Id;
                    Parallel.ForEach(fileStop.Jobs, x => x.StopId = originalStop.Id);
                    Parallel.ForEach(s.Jobs, x => x.StopId = originalStop.Id);

                    stopImportStatuses.Add(new StopImportStatus(fileStop, StopImportStatus.Status.Updated, originalStopIdentifiers));
                    s.Id = fileStop.Id;
                }
                else
                {
                    var message = $"Ignoring Stop update. Stop is Complete  " +
                                  $"stop id ({originalStop.Id}) " +
                                  $"identifier ({originalStop.Identifier()}), " +
                                  $"route header Id ({originalStop.RouteHeaderId})";
                    logger.LogDebug(message);
                    stopImportStatuses.Add(new StopImportStatus(originalStop, StopImportStatus.Status.IgnoredAsCompleted, originalStop.CloneStopIdentifiers()));
                }

            }

            ImportJobs(fileRouteHeader, stopImportStatuses, importMapper, importCommands);

            //TODO: move this to the end of the routeheader import
            DeleteStopsNotInFile(existingRouteStopsFromDb, fileRouteHeader, importCommands);
        }

        private void DeleteStopsNotInFile(IEnumerable<Stop> existingRouteStopsFromDb, RouteHeader fileRouteHeader, IImportCommands importCommands)
        {
            var routeFileCommands = importCommands as IAdamFileImportCommands;
            // Only delete Stops on Route File import
            routeFileCommands?.DeleteStopsNotInFile(existingRouteStopsFromDb, fileRouteHeader.Stops);
        }

        private bool IsEpodImport(IImportCommands importCommands)
        {
            var epodComs = importCommands as IEpodFileImportCommands;
            return epodComs != null;
        }

        private void AddHeaderInformationToStops(RouteHeader header)
        {
            Parallel.ForEach(header.Stops,
                x =>
                {
                    x.RouteHeaderId = header.Id;
                    x.RouteHeaderCode = header.RouteNumber;
                    x.DeliveryDate = header.RouteDate;
                });
        }

        public void ImportJobs(
            RouteHeader routeHeader,
            IList<StopImportStatus> stopImportStatuses,
            IImportMapper importMapper,
            IImportCommands importCommands)
        {
            var branchId = routeHeader.RouteOwnerId;
            var existingRouteJobIdAndStopId = jobRepository.GetJobStopsByRouteHeaderId(routeHeader.Id).ToList();
            var jobs =
                stopImportStatuses.Where(x => x.ImportStatus != StopImportStatus.Status.IgnoredAsCompleted)
                .Select(s => s.Stop).SelectMany(j => j.Jobs).ToList();

            var existingJobsBothSources = GetExistingJobs(branchId, jobs);

            List<int> updateJobIds = new List<int>();

            foreach (var job in jobs.Where(fj => fj.IncludeJobTypeInImport()))
            {
                var originalJob = FindOriginalJob(existingJobsBothSources, job);

                if (originalJob == null)
                {
                    jobService.SetInitialJobStatus(job);
                    job.ResolutionStatus = ResolutionStatus.Imported;
                    jobRepository.Save(job);
                    jobRepository.SaveJobResolutionStatus(job);
                    updateJobIds.Add(job.Id);

                    job.JobDetails.ForEach(
                        x =>
                        {
                            x.JobId = job.Id;
                            x.ShortsStatus = JobDetailStatus.Res;
                            x.JobDetailReason = JobDetailReason.NotDefined;
                            x.JobDetailSource = JobDetailSource.NotDefined;
                        });

                    this.ImportJobDetails(job.JobDetails);

                    DoAfterJobCreation(importCommands, job, routeHeader);
                }

                // Update Existing's
                else if (originalJob.CanWeUpdateJobOnImport())
                {
                    bool isJobReplanned = IsJobReplanned(stopImportStatuses, job, originalJob);

                    originalJob.StopId = job.StopId;
                    importCommands.UpdateExistingJob(job, originalJob, routeHeader, isJobReplanned);
                    updateJobIds.Add(originalJob.Id);
                    job.Id = originalJob.Id;
                    job.JobStatus = originalJob.JobStatus;
                }
                else
                {
                    var message = $"Ignoring Job update. Job is Complete  " +
                                  $"job id ({originalJob.Id}) " +
                                  $"identifier ({job.Identifier()}), " +
                                  $"branch id  ({branchId})";
                    logger.LogDebug(message);
                }

                var fileJob = routeHeader.Stops
                    .SelectMany(p => p.Jobs)
                    .FirstOrDefault(p => p.PickListRef == job.PickListRef
                        && (p.JobTypeCode == job.JobTypeCode || p.JobTypeCodeTransend == job.JobTypeCode)
                        && p.StopId == job.StopId);

                if (fileJob != null)
                {
                    fileJob.Id = job.Id;
                    fileJob.JobStatus = job.JobStatus;
                }
            }

            // updates Location/Activity/LineItem/Bag tables from imported data
            importCommands.PostJobImport(updateJobIds);
            var completedStops = stopImportStatuses.Where(x => x.ImportStatus == StopImportStatus.Status.IgnoredAsCompleted).Select(s => s.Stop).ToList();
            //Delete Jobs Not In File
            var jobsToBeDeleted = importCommands.GetJobsToBeDeleted(existingRouteJobIdAndStopId, existingJobsBothSources, completedStops).ToList();

            DeleteJobs(jobsToBeDeleted);
        }

        public virtual bool IsJobReplanned(IList<StopImportStatus> stopImportStatuses, Job fileJob, Job originalJob)
        {
            bool isJobReplanned = false;

            if (originalJob.WellStatus == WellStatus.Bypassed)
            {
                isJobReplanned = HasJobMovedStops(originalJob, fileJob);

                if (!isJobReplanned)
                {
                    var stopImportStatus = stopImportStatuses.First(s => s.Stop.Id == fileJob.StopId);
                    isJobReplanned = HasStopBeenReplanned(stopImportStatus.Stop, stopImportStatus.OriginalStopIdentifiers);
                }
            }
            return isJobReplanned;
        }

        public virtual bool HasJobMovedStops(Job originalJob, Job fileJob)
        {
            return originalJob.StopId != fileJob.StopId;
        }

        
        // A stop is considered replanned if it has
        // Moved to a different route
        public virtual bool HasStopBeenReplanned(IStopMoveIdentifiers newIdentifier, IStopMoveIdentifiers original)
        {
            if (newIdentifier.RouteHeaderId != original.RouteHeaderId)
            {
                return true;
            }

            //int originalPlannedStopNumber;
            //if (!int.TryParse(original.PlannedStopNumber, out originalPlannedStopNumber))
            //{
            //    return false;
            //}

            //int newPlannedStopNumber;
            //if (!int.TryParse(newIdentifier.PlannedStopNumber, out newPlannedStopNumber))
            //{
            //    return false;
            //}

            //if (originalPlannedStopNumber < newPlannedStopNumber)
            //{
            //    return true;
            //}

            return false;
        }

        private void DoAfterJobCreation(IImportCommands importCommands, Job job, RouteHeader routeHeader)
        {
            //only do this if we are doing an epod update
            var epodimportCommands = importCommands as IEpodFileImportCommands;
            epodimportCommands?.AfterJobCreation(job, job, routeHeader);
        }

        private IList<Stop> GetExistingStops(IList<string> transportOrderReference)
        {
            var stopIds = stopRepository.GetStopByTransportOrderRefIncludingSoftDeleted(transportOrderReference).ToList();
            stopRepository.ReinstateStopSoftDeletedByImport(stopIds);
            return stopRepository.GetByIds(stopIds);
        }

        private Stop FindOriginalStop(IList<Stop> existingStops, Stop stop)
        {
            return existingStops.FirstOrDefault(x => x.TransportOrderReference == stop.TransportOrderReference);
        }

        public void DeleteJobs(IList<Job> jobsToBeDeleted)
        {
            foreach (var jobToDelete in jobsToBeDeleted)
            {
                if (jobToDelete.CanWeUpdateJobOnImport())
                {
                    this.jobRepository.CascadeSoftDeleteJobs(new[] { jobToDelete.Id }, true);
                }
            }
        }

        private IList<Job> GetExistingJobs(int branchId, IList<Job> jobs)
        {
            var existing = jobRepository.
                GetExistingJobsIdsIncludingSoftDeleted(branchId, jobs).ToList();

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

                        this.jobDetailDamageRepository.Save(x);
                    });
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