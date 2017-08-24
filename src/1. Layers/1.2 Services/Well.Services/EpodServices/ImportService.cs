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

        //TODO: Unit Tests
        public virtual void ImportStops(RouteHeader fileRouteHeader, IImportMapper importMapper, IImportCommands importCommands)
        {
            AddHeaderInformationToStops(fileRouteHeader);

            var existingRouteStopsFromDb = stopRepository.GetStopByRouteHeaderId(fileRouteHeader.Id);

            IList<Stop> existingStopsBothSources = GetExistingStops(fileRouteHeader.Stops.Select(s => s.TransportOrderReference).Distinct().ToList());

            var savedStops = new List<Stop>();

            //loop throught all stops in the file
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
                    savedStops.Add(fileStop);
                }
                // Update Existing
                else if (!originalStop.HasStopBeenCompleted())
                {
                    originalStop.Previously = originalStop.GetPreviously(fileStop);

                    importMapper.MapStop(fileStop, originalStop);
                    stopRepository.Update(originalStop);

                    fileStop.Jobs.ForEach(x => x.StopId = originalStop.Id);
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

            ImportJobs(fileRouteHeader, savedStops.SelectMany(j => j.Jobs).ToList(), importMapper, importCommands);
            DeleteStopsNotInFile(existingRouteStopsFromDb, fileRouteHeader, importCommands);

        }

        private void DeleteStopsNotInFile(IEnumerable<Stop> existingRouteStopsFromDb, RouteHeader fileRouteHeader, IImportCommands importCommands)
        {
            var routeFileCommands = importCommands as IAdamFileImportCommands;
            // Only delete Stops on Route File import
            if (routeFileCommands != null)
            {
                routeFileCommands.DeleteStopsNotInFile(existingRouteStopsFromDb, fileRouteHeader.Stops);
            }
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
            IList<Job> fileJobs,
            IImportMapper importMapper,
            IImportCommands importCommands)
        {
            var branchId = routeHeader.RouteOwnerId;
            var existingRouteJobIdAndStopId = jobRepository.GetJobStopsByRouteHeaderId(routeHeader.Id).ToList();
            var existingJobsBothSources = GetExistingJobs(branchId, fileJobs);
            
            List<int> updateJobIds = new List<int>();
            foreach (var job in fileJobs)
            {
                var originalJob = FindOriginalJob(existingJobsBothSources, job);

                if (originalJob == null)
                {
                    jobService.SetInitialJobStatus(job);
                    job.ResolutionStatus = ResolutionStatus.Imported;
                    jobRepository.Save(job);
                    jobRepository.SetJobResolutionStatus(job.Id, job.ResolutionStatus.Description);
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

                // Update Existings
                else if (originalJob.CanWeUpdateJobOnImport())
                {
                    originalJob.StopId = job.StopId;
                    importCommands.UpdateExistingJob(job, originalJob, routeHeader);
                    updateJobIds.Add(originalJob.Id);
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

            // updates Location/Activity/LineItem/Bag tables from imported data
            importCommands.PostJobImport(updateJobIds);

            //Delete Jobs Not In File
            var jobsToBeDeleted = importCommands.GetJobsToBeDeleted(existingRouteJobIdAndStopId, existingJobsBothSources).ToList();
           
            DeleteJobs(jobsToBeDeleted);
        }

        private void DoAfterJobCreation(IImportCommands importCommands, Job job, RouteHeader routeHeader)
        {
            //only do this if we are doing an epod update
            var epodimportCommands = importCommands as IEpodFileImportCommands;
            if (epodimportCommands != null)
            {
                epodimportCommands.AfterJobCreation(job, job, routeHeader);
            }
        }

        private IList<Stop> GetExistingStops(List<string> transportOrderReference)
        {
            var stopIds = stopRepository.GetStopByTransportOrderRefIncludingSoftDeleted(transportOrderReference).ToList();
            stopRepository.ReinstateStopSoftDeletedByImport(stopIds);
            return stopRepository.GetByIds(stopIds);
        }

        private Stop FindOriginalStop(IList<Stop> existingStops, Stop stop)
        {
            return existingStops.FirstOrDefault(x => x.TransportOrderReference == stop.TransportOrderReference);
        }

        private void DeleteJobs(List<Job> jobsToBeDeleted)
        {
            foreach (var jobToDelete in jobsToBeDeleted)
            {
                if (jobToDelete.IsDocumentDelivery() || jobToDelete.CanWeUpdateJobOnImport())
                {
                    this.jobRepository.CascadeSoftDeleteJobs(new[] { jobToDelete.Id }, true);
                }
            }
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

        private Job FindOriginalJob(IList<Job> existingJobs, Job job)
        {
            return existingJobs.FirstOrDefault(x =>
                x.Identifier() == job.Identifier()
            );
        }
    }
}