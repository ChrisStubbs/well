namespace PH.Well.Services.EpodServices
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain;
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

            IList<Stop> existingStopsBothSources = GetExistingStops(fileRouteHeader.Stops.Select(s => s.TransportOrderReference).Distinct().ToList());

            var stopImportStatuses = new List<StopImportStatus>();

            //loop through all stops in the file
            foreach (var s in fileRouteHeader.Stops)
            {
                Stop fileStop = AutoMapperConfig.Mapper.Map<StopDTO, Stop>(s);

                var originalStop = FindOriginalStop(existingStopsBothSources, fileStop);

                //Is New
                if (originalStop == null)
                {
                    stopRepository.Save(fileStop);
                    fileStop.Jobs.ForEach(x => x.StopId = fileStop.Id);
                    fileStop.Account.StopId = fileStop.Id;
                    accountRepository.Save(fileStop.Account);
                    stopImportStatuses.Add(new StopImportStatus(fileStop, StopImportStatus.Status.New));
                }
                // Update Existing
                else if (!originalStop.HasStopBeenCompleted())
                {
                    originalStop.Previously = originalStop.GetPreviously(fileStop);
                    //bool stopHasMoved = originalStop.TransportOrderReference != fileStop.TransportOrderReference;
                        //originalStop.HasMoved(fileStop);
                    importMapper.MapStop(fileStop, originalStop);
                    stopRepository.Update(originalStop);
                    fileStop.Id = originalStop.Id;
                    fileStop.Jobs.ForEach(x => x.StopId = originalStop.Id);

                    stopImportStatuses.Add(new StopImportStatus(fileStop, StopImportStatus.Status.Updated));
                }
                else
                {
                    var message = $"Ignoring Stop update. Stop is Complete  " +
                                  $"stop id ({originalStop.Id}) " +
                                  $"identifier ({originalStop.Identifier()}), " +
                                  $"route header Id ({originalStop.RouteHeaderId})";
                    logger.LogDebug(message);
                    stopImportStatuses.Add(new StopImportStatus(originalStop, StopImportStatus.Status.IgnoredAsCompleted));
                }

            }

            ImportJobs(fileRouteHeader, stopImportStatuses, importMapper, importCommands);

            DeleteStopsNotInFile(existingRouteStopsFromDb, fileRouteHeader, importCommands);
            // Batch update WellStatus for stops
            UpdateWellStatusForStops(
                stopImportStatuses.Where(x => x.ImportStatus != StopImportStatus.Status.IgnoredAsCompleted)
                .Select(x => x.Stop.Id).ToArray());
        }

        private void UpdateWellStatusForStops(params int[] stopIds)
        {
            foreach (var id in stopIds)
            {
                stopService.ComputeWellStatus(id);
            }
        }

        private void DeleteStopsNotInFile(IEnumerable<Stop> existingRouteStopsFromDb, RouteHeader fileRouteHeader, IImportCommands importCommands)
        {
            var routeFileCommands = importCommands as IAdamFileImportCommands;
            // Only delete Stops on Route File import
            routeFileCommands?.DeleteStopsNotInFile(existingRouteStopsFromDb, fileRouteHeader.Stops);
        }

        private void AddHeaderInformationToStops(RouteHeader header)
        {
            header.Stops.ForEach(
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
            var fileJobs =
                stopImportStatuses.Where(x => x.ImportStatus != StopImportStatus.Status.IgnoredAsCompleted)
                .Select(s => s.Stop).SelectMany(j => j.Jobs).ToList();

            var existingJobsBothSources = GetExistingJobs(branchId, fileJobs);

            List<int> updateJobIds = new List<int>();

            foreach (var fileJob in fileJobs.Where(fj => fj.IncludeJobTypeInImport()))
            {
                var originalJob = FindOriginalJob(existingJobsBothSources, fileJob);

                if (originalJob == null)
                {
                    jobService.SetInitialJobStatus(fileJob);
                    fileJob.ResolutionStatus = ResolutionStatus.Imported;
                    jobRepository.Save(fileJob);
                    jobRepository.SaveJobResolutionStatus(fileJob);
                    updateJobIds.Add(fileJob.Id);

                    fileJob.JobDetails.ForEach(
                        x =>
                        {
                            x.JobId = fileJob.Id;
                            x.ShortsStatus = JobDetailStatus.Res;
                            x.JobDetailReason = JobDetailReason.NotDefined;
                            x.JobDetailSource = JobDetailSource.NotDefined;
                        });

                    this.ImportJobDetails(fileJob.JobDetails);

                    DoAfterJobCreation(importCommands, fileJob, routeHeader);
                }

                // Update Existings
                else if (originalJob.CanWeUpdateJobOnImport())
                {
                    bool jobHasMovedStops = originalJob.StopId != fileJob.StopId;
                    originalJob.StopId = fileJob.StopId;
                    importCommands.UpdateExistingJob(fileJob, originalJob, routeHeader,  jobHasMovedStops);
                    updateJobIds.Add(originalJob.Id);
                }
                else
                {
                    var message = $"Ignoring Job update. Job is Complete  " +
                                  $"job id ({originalJob.Id}) " +
                                  $"identifier ({fileJob.Identifier()}), " +
                                  $"branch id  ({branchId})";
                    logger.LogDebug(message);
                }
            }

            // updates Location/Activity/LineItem/Bag tables from imported data
            importCommands.PostJobImport(updateJobIds);
            var completedStops = stopImportStatuses.Where(x => x.ImportStatus == StopImportStatus.Status.IgnoredAsCompleted).Select(s => s.Stop).ToList();
            //Delete Jobs Not In File
            var jobsToBeDeleted = importCommands.GetJobsToBeDeleted(existingRouteJobIdAndStopId, existingJobsBothSources, completedStops).ToList();

            DeleteJobs(jobsToBeDeleted);
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

        private Job FindOriginalJob(IList<Job> existingJobs, Job job)
        {
            return existingJobs.FirstOrDefault(x =>
                x.Identifier() == job.Identifier()
            );
        }

    }
}