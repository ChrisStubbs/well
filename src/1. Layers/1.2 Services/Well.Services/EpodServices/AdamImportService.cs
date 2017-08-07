namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Common;
    using Common.Contracts;
    using Domain;
    using Domain.Enums;
    using Repositories.Contracts;
    using Contracts;
    using Domain.Constants;
    using static Domain.Mappers.AutoMapperConfig;

    public class AdamImportService : IAdamImportService
    {
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IStopRepository stopRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IJobRepository jobRepository;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IJobDetailDamageRepository jobDetailDamageRepository;
        private readonly IJobService jobStatusService;
        private readonly ILogger logger;
        private readonly IEventLogger eventLogger;
        private readonly IPostImportRepository postImportRepository;

        public AdamImportService(IRouteHeaderRepository routeHeaderRepository, 
            IStopRepository stopRepository, 
            IAccountRepository accountRepository, 
            IJobRepository jobRepository, 
            IJobDetailRepository jobDetailRepository, 
            IJobDetailDamageRepository jobDetailDamageRepository,
            IJobService jobStatusService,
            ILogger logger, 
            IEventLogger eventLogger,
            IPostImportRepository postImportRepository)
        {
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.accountRepository = accountRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
            this.jobStatusService = jobStatusService;
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.postImportRepository = postImportRepository;
        }

        public void Import(RouteDelivery route)
        {
            foreach (var header in route.RouteHeaders)
            {
                var existingRouteHeader = this.routeHeaderRepository.GetByNumberDateBranch(
                    header.RouteNumber,
                    header.RouteDate.Value,
                    header.RouteOwnerId); 

                if (existingRouteHeader != null)
                {
                    this.logger.LogDebug($"Will not import route header as already exists from route number ({header.RouteNumber}), route date ({header.RouteDate.Value}), branch ({header.RouteOwnerId})");
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"Will not import route header as already exists from route number ({header.RouteNumber}), route date ({header.RouteDate.Value}), branch ({header.RouteOwnerId})",
                        EventId.ImportIgnored);
                    continue;
                }

                this.ImportRouteHeader(header, route.RouteId);
            }
        }

        public void ImportRouteHeader(RouteHeader header, int routeId)
        {
            header.RouteStatusDescription = "Not Started";
            header.RoutesId = routeId;
            header.RouteOwnerId = string.IsNullOrWhiteSpace(header.RouteOwner)
                                    ? (int)Branches.NotDefined
                                    : (int)Enum.Parse(typeof(Branches), header.RouteOwner, true);

            this.routeHeaderRepository.Save(header);

            header.Stops.ForEach(
                x =>
                {
                    x.RouteHeaderId = header.Id;
                    x.RouteHeaderCode = header.RouteNumber;
                    x.DeliveryDate = header.RouteDate;
                });

            this.ImportStops(header.Stops);
        }

        private void ImportStops(IEnumerable<StopDTO> stops)
        {
            foreach (var s in stops)
            {
                var stop = Mapper.Map<StopDTO, Stop>(s);

                try
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        this.stopRepository.Save(stop);

                        stop.Account.StopId = stop.Id;

                        stop.Jobs.ForEach(x => x.StopId = stop.Id);

                        this.accountRepository.Save(stop.Account);

                        this.ImportJobs(stop.Jobs);
                        // updates Location/Activity/LineItem/Bag tables from imported data
                        this.postImportRepository.PostImportUpdate(stop.Jobs.Select(x=> x.Id).Distinct());

                        transactionScope.Complete();
                    }
                }
                catch (Exception exception)
                {
                    this.logger.LogError($"Stop has an error on import! Stop Id ({stop.Id})", exception);
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"Stop has an error on import! Stop Id ({stop.Id})",
                        EventId.ImportStopException);
                }
            }
        }

        private void ImportJobs(IEnumerable<Job> jobs)
        {
            foreach (var job in jobs)
            {
                this.jobStatusService.SetInitialJobStatus(job);
                job.ResolutionStatus = ResolutionStatus.Imported;
                this.jobRepository.Save(job);
                this.jobRepository.SetJobResolutionStatus(job.Id, job.ResolutionStatus.Description);

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
    }
}