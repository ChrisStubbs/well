namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
    using System.Transactions;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class AdamImportService : IAdamImportService
    {
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IStopRepository stopRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IJobRepository jobRepository;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IJobDetailDamageRepository jobDetailDamageRepository;

        private readonly ILogger logger;
        private readonly IEventLogger eventLogger;
        private const string CurrentUser = "AdamImport";

        public AdamImportService(IRouteHeaderRepository routeHeaderRepository, 
            IStopRepository stopRepository, IAccountRepository accountRepository, 
            IJobRepository jobRepository, IJobDetailRepository jobDetailRepository, 
            IJobDetailDamageRepository jobDetailDamageRepository,
            ILogger logger, IEventLogger eventLogger)
        {
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.accountRepository = accountRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository.CurrentUser = CurrentUser;
            this.stopRepository.CurrentUser = CurrentUser;
            this.accountRepository.CurrentUser = CurrentUser;
            this.jobRepository.CurrentUser = CurrentUser;
            this.jobDetailRepository.CurrentUser = CurrentUser;
            this.jobDetailDamageRepository.CurrentUser = CurrentUser;
        }

        public void Import(RouteDelivery route)
        {
            foreach (var header in route.RouteHeaders)
            {
                var existingRouteHeader = this.routeHeaderRepository.GetByNumberDateBranch(
                    header.RouteNumber,
                    header.RouteDate.Value,
                    header.StartDepot);

                if (existingRouteHeader != null)
                {
                    this.logger.LogDebug($"Will not import route header as already exists from route number ({header.RouteNumber}), route date ({header.RouteDate.Value}), branch ({header.StartDepot})");
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"Will not import route header as already exists from route number ({header.RouteNumber}), route date ({header.RouteDate.Value}), branch ({header.StartDepot})",
                        7776);
                    continue;
                }

                header.RoutesId = route.RouteId;
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
        }

        private void ImportStops(IEnumerable<Stop> stops)
        {
            foreach (var stop in stops)
            {
                try
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        this.stopRepository.Save(stop);

                        stop.Account.StopId = stop.Id;

                        stop.Jobs.ForEach(x => x.StopId = stop.Id);

                        this.accountRepository.Save(stop.Account);

                        this.ImportJobs(stop.Jobs);

                        transactionScope.Complete();
                    }
                }
                catch (Exception exception)
                {
                    this.logger.LogError($"Stop has an error on import! Stop Id ({stop.Id})", exception);
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"Stop has an error on import! Stop Id ({stop.Id})",
                        9853);
                }
            }
        }

        private void ImportJobs(IEnumerable<Job> jobs)
        {
            foreach (var job in jobs)
            {
                this.jobRepository.Save(job);

                job.JobDetails.ForEach(
                    x =>
                    {
                        x.JobId = job.Id;
                        x.JobDetailStatusId = (int)JobDetailStatus.UnRes;
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

                detail.JobDetailDamages.ForEach(x => x.JobDetailId = detail.Id);

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