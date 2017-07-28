namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Common;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Constants;
    using Domain.Enums;
    using Repositories.Contracts;

    public class RouteImportService : IAdamImportService
    {
        private readonly ILogger logger;
        private readonly IEventLogger eventLogger;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IStopRepository stopRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IJobRepository jobRepository;
        private readonly IJobService jobService;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IJobDetailDamageRepository jobDetailDamageRepository;

        public RouteImportService(
            ILogger logger,
            IEventLogger eventLogger,
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IAccountRepository accountRepository,
            IJobRepository jobRepository,
            IJobService jobService,
            IJobDetailRepository jobDetailRepository,
            IJobDetailDamageRepository jobDetailDamageRepository
            )
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.accountRepository = accountRepository;
            this.jobRepository = jobRepository;
            this.jobService = jobService;
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
        }

        public void Import(RouteDelivery route)
        {
            foreach (var header in route.RouteHeaders)
            {
                //TODO: PUT IN TRANSACTION 
                this.ImportRouteHeader(header, route.RouteId);
            }
        }

        public void ImportRouteHeader(RouteHeader header, int routeId)
        {
            var existingRouteHeader = this.routeHeaderRepository.GetByNumberDateBranch(
                header.RouteNumber,
                header.RouteDate.Value,
                header.RouteOwnerId);

            if (existingRouteHeader != null)
            {
                if (header.RouteStatusCode.Equals(RouteStatusCode.Completed))
                {
                    var message = $"Ignoring Route update. Route is Complete  " +
                                  $"route header id ({header.Id}) " +
                                  $"number ({header.RouteNumber}), " +
                                  $"route date ({header.RouteDate.Value}), " +
                                  $"branch ({header.RouteOwnerId})";
                    logger.LogDebug(message);
                    this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, message, EventId.ImportIgnored);
                    return;
                }

                header.Id = existingRouteHeader.Id;
                routeHeaderRepository.Update(header);
                logger.LogDebug(
                    $"Updating Route  " +
                    $"route header id ({header.Id}) " +
                    $"number ({header.RouteNumber}), " +
                    $"route date ({header.RouteDate.Value}), " +
                    $"branch ({header.RouteOwnerId})"
                );
            }
            else
            {
                header.RouteStatusDescription = "Not Started";
                header.RoutesId = routeId;
                header.RouteOwnerId = string.IsNullOrWhiteSpace(header.RouteOwner)
                    ? (int)Branches.NotDefined
                    : (int)Enum.Parse(typeof(Branches), header.RouteOwner, true);

                routeHeaderRepository.Save(header);

                logger.LogDebug(
                    $"Inserting Route  " +
                    $"route header id ({header.Id}) " +
                    $"number ({header.RouteNumber}), " +
                    $"route date ({header.RouteDate.Value}), " +
                    $"branch ({header.RouteOwnerId})"
                );
            }

            ImportStops(header);
        }

        private void ImportStops(RouteHeader fileRouteHeader)
        {
            var existRouteStops = stopRepository.GetStopByRouteHeaderId(fileRouteHeader.Id);

            var existingStops = stopRepository.GetByTransportOrderReferences(
                        fileRouteHeader.Stops.Select(s => s.TransportOrderReference).Distinct().ToList()
                ).ToArray();

            var fileStops = new List<Stop>();
            foreach (var s in fileRouteHeader.Stops)
            {
                Stop fileStop = Mapper.Map<StopDTO, Stop>(s);
                fileStops.Add(fileStop);

                Stop originalStop = GetOriginalStop(existingStops, fileStop);

                fileStop.Id = originalStop?.Id ?? 0;

                //Is New
                if (fileStop.IsTransient())
                {
                    stopRepository.Save(fileStop);
                    fileStop.Jobs.ForEach(x => x.StopId = fileStop.Id);
                    fileStop.Account.StopId = fileStop.Id;
                    accountRepository.Save(fileStop.Account);
                }
                // Update Existing
                else if (!HasStopBeenCompleted(originalStop))
                {
                    fileStop.SetPreviously(originalStop);
                    stopRepository.Update(fileStop);
                    fileStop.Account.StopId = fileStop.Id;
                    accountRepository.Update(fileStop.Account);
                }
            }

            ImportJobs(fileRouteHeader.Id, fileRouteHeader.RouteOwnerId, fileStops.SelectMany(j => j.Jobs).ToList());

            //Delete Stops Not In File
            var stopsToBeDeleted = existRouteStops.Where(x => !fileRouteHeader.Stops.Select(s => x.TransportOrderReference).Distinct().Contains(x.TransportOrderReference));
            foreach (var existingRouteStop in stopsToBeDeleted)
            {
                if (!HasStopBeenCompleted(existingRouteStop))
                {
                    existingRouteStop.DateDeleted = DateTime.Now;
                    stopRepository.Update(existingRouteStop);
                }
            }
        }

        private void ImportJobs(int routeHeaderId, int branchId, IList<Job> jobs)
        {
            var existingJobs = jobRepository.GetExistingJobs(branchId, jobs.Where(x => x.PickListRef != Job.DocumentPickListReference)).ToList();
            var existingJobsInStops = jobRepository.GetJobIdsByRouteHeaderId(routeHeaderId);

            foreach (var job in jobs)
            {
                var originalJob = GetOriginalJob(existingJobs, job);
                job.Id = originalJob?.Id ?? 0;

                if (job.IsTransient())
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

                    //TODO: Add The ImportJobDetails
                    this.ImportJobDetails(job.JobDetails);
                }
                // Update Existing
                else if (CanWeUpdateJob(originalJob))
                {
                    jobRepository.Update(job);
                    job.JobDetails.ForEach(
                        x =>
                        {
                            x.JobId = job.Id;
                        });
                    //TODO: IF EPOD File Update JobDetails
                    // if epod update with epod details
                }
            }

            //Delete Jobs Not In File
            var jobsToBeDeleted = jobRepository.GetByIds(existingJobsInStops.Where(x => !existingJobs.Select(ej => ej.Id).Contains(x)));
            foreach (var jobToDelete in jobsToBeDeleted)
            {
                if (CanWeUpdateJob(jobToDelete))
                {
                    jobToDelete.DateDeleted = DateTime.Now;
                    jobRepository.Update(jobToDelete);
                }
            }

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

        private bool HasStopBeenCompleted(Stop stop)
        {
            return stop.WellStatus == WellStatus.Complete;
        }

        private Stop GetOriginalStop(IList<Stop> existingStops, Stop stop)
        {
            return existingStops.FirstOrDefault(x => x.TransportOrderReference == stop.TransportOrderReference);
        }

        private Job GetOriginalJob(IList<Job> existingJobs, Job job)
        {
            return existingJobs.FirstOrDefault(x =>
                x.PhAccount == job.PhAccount
                && x.PickListRef == job.PickListRef
                && x.JobTypeCode == job.JobTypeCode
            );
        }
    }
}