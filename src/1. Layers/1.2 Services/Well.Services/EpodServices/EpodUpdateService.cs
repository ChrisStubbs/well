namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;

    using Domain.ValueObjects;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class EpodUpdateService : IEpodUpdateService
    {
        private readonly ILogger logger;

        private readonly IEventLogger eventLogger;

        private readonly IRouteHeaderRepository routeHeaderRepository;

        private readonly IStopRepository stopRepository;

        private readonly IJobRepository jobRepository;

        private readonly IJobDetailRepository jobDetailRepository;

        private readonly IJobDetailDamageRepository jobDetailDamageRepository;

        private readonly IExceptionEventRepository exceptionEventRepository;

        private readonly IPodTransactionFactory podTransactionFactory;

        private readonly IRouteMapper mapper;

        private readonly IAdamImportService adamImportService;

        private readonly IJobStatusService jobStatusService;

        private readonly IUserNameProvider userNameProvider;

        private const string UpdatedBy = "EpodUpdate";

        public EpodUpdateService(
            ILogger logger,
            IEventLogger eventLogger,
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IJobRepository jobRepository,
            IJobDetailRepository jobDetailRepository,
            IJobDetailDamageRepository jobDetailDamageRepository,
            IExceptionEventRepository exceptionEventRepository,
            IRouteMapper mapper,
            IAdamImportService adamImportService,
            IPodTransactionFactory podTransactionFactory,
            IJobStatusService jobStatusService,
            IUserNameProvider userNameProvider)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.jobRepository = jobRepository;
            this.jobDetailRepository = jobDetailRepository;
            this.jobDetailDamageRepository = jobDetailDamageRepository;
            this.exceptionEventRepository = exceptionEventRepository;
            this.mapper = mapper;
            this.adamImportService = adamImportService;
            this.podTransactionFactory = podTransactionFactory;
            this.jobStatusService = jobStatusService;
            this.userNameProvider = userNameProvider;
        }

        public void Update(RouteDelivery route)
        {
            foreach (var header in route.RouteHeaders)
            {
                var existingHeader = this.routeHeaderRepository.GetRouteHeaderByRoute(
                    header.RouteNumber.Substring(2),
                    header.RouteDate);

                if (existingHeader == null)
                {
                    this.adamImportService.ImportRouteHeader(header, route.RouteId);
                    continue;
                }

                this.mapper.Map(header, existingHeader);

                this.routeHeaderRepository.Update(existingHeader);

                int branchId;

                if (int.TryParse(existingHeader.StartDepotCode, out branchId))
                {
                    this.UpdateStops(header.Stops, branchId);
                }
                else
                {
                    this.logger.LogDebug(
                        $"Start depot code is not an int... Depot code passed in from transend is ({existingHeader.StartDepotCode})");
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"Start depot code is not an int... Depot code passed in from transend is ({existingHeader.StartDepotCode})",
                        9682);
                }
            }
        }

        private void UpdateStops(IEnumerable<Stop> stops, int branchId)
        {
            foreach (var stop in stops)
            {
                try
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        var job = stop.Jobs.First();
                        var existingStop = this.stopRepository.GetByJobDetails(job.PickListRef, job.PhAccount);

                        if (existingStop == null)
                        {
                            this.logger.LogDebug(
                                $"Existing stop not found with transport order reference {stop.TransportOrderReference}");
                            this.eventLogger.TryWriteToEventLog(
                                EventSource.WellAdamXmlImport,
                                $"Existing stop not found with transport order reference {stop.TransportOrderReference}",
                                7666);

                            continue;
                        }

                        this.mapper.Map(stop, existingStop);

                        this.stopRepository.Update(existingStop);

                        this.UpdateJobs(stop.Jobs, existingStop.Id, branchId);

                        transactionScope.Complete();
                    }
                }
                catch (Exception exception)
                {
                    this.logger.LogError(
                        $"Stop has an error on Epod update! Stop Id ({stop.Id}), Transport order reference ({stop.TransportOrderReference})",
                        exception);
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"Stop has an error on Epod update! Stop Id ({stop.Id}), Transport order reference ({stop.TransportOrderReference})",
                        9859);
                }
            }
        }

        private void UpdateJobs(IEnumerable<Job> jobs, int stopId, int branchId)
        {
            foreach (var job in jobs)
            {
                var existingJob = this.jobRepository.GetByAccountPicklistAndStopId(
                    job.PhAccount,
                    job.PickListRef,
                    stopId);

                if (existingJob == null)
                {
                    continue;
                }

                this.mapper.Map(job, existingJob);

                this.jobStatusService.DetermineStatus(existingJob, branchId);

                if (!string.IsNullOrWhiteSpace(job.GrnNumber) && existingJob.GrnProcessType == 1)
                {
                    var grnEvent = new GrnEvent { Id = existingJob.Id, BranchId = branchId };

                    this.exceptionEventRepository.InsertGrnEvent(grnEvent);
                }

                this.UpdateJobDetails(
                    job.JobDetails,
                    existingJob.Id,
                    string.IsNullOrWhiteSpace(existingJob.InvoiceNumber));

                //TODO POD event
                var pod = existingJob.ProofOfDelivery.GetValueOrDefault();

                if (pod == (int)ProofOfDelivery.CocaCola)
                {
                    //TODO LRS customer (lucozade) 
                    //build pod transaction
                    var podEvent = new PodEvent
                    {
                        BranchId = branchId,
                        Id = existingJob.Id
                    };
                    this.exceptionEventRepository.InsertPodEvent(podEvent);
                }

                this.jobRepository.Update(existingJob);
            }
        }

        private void UpdateJobDetails(IEnumerable<JobDetail> jobDetails, int jobId, bool invoiceOutstanding)
        {
            foreach (var detail in jobDetails)
            {
                var existingJobDetail = this.jobDetailRepository.GetByJobLine(jobId, detail.LineNumber);

                detail.ShortsStatus = detail.ShortQty == 0 ? JobDetailStatus.Res : JobDetailStatus.UnRes;

                if (existingJobDetail == null)
                {
                    detail.JobId = jobId;

                    this.jobDetailRepository.Save(detail);

                    continue;
                }
                
                this.mapper.Map(detail, existingJobDetail);

                detail.SkuGoodsValue = existingJobDetail.SkuGoodsValue;

                if (detail.ShortQty > 0)
                {
                    detail.JobDetailReason = JobDetailReason.NotDefined;
                    detail.JobDetailSource = JobDetailSource.NotDefined;

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
                if (!string.IsNullOrWhiteSpace(damage.Reason.Description) &&
                    damage.Reason.Description.ToLower().Contains("short"))
                {
                    continue;
                }

                damage.JobDetailReason = JobDetailReason.NotDefined;
                damage.DamageStatus = damage.Qty == 0 ? JobDetailStatus.Res : JobDetailStatus.UnRes;
                this.jobDetailDamageRepository.Save(damage);
            }
        }
    }
}