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
        private readonly IRouteMapper mapper;
        private readonly IAdamImportService adamImportService;
        private readonly IDeliveryStatusService deliveryStatusService;
        private readonly IUserNameProvider userNameProvider;

        private const string UpdatedBy = "EpodUpdate";

        private List<int> ProofOfDeliveryList = new List<int> { 1 , 8}; 

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
            IDeliveryStatusService deliveryStatusService,
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
            this.deliveryStatusService = deliveryStatusService;
            this.userNameProvider = userNameProvider;

            //this.routeHeaderRepository.CurrentUser = UpdatedBy;
            //this.stopRepository.CurrentUser = UpdatedBy;
            //this.jobRepository.CurrentUser = UpdatedBy;
            //this.jobDetailRepository.CurrentUser = UpdatedBy;
            //this.jobDetailDamageRepository.CurrentUser = UpdatedBy;
            //this.exceptionEventRepository.CurrentUser = UpdatedBy;
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

                this.UpdateStops(header.Stops, existingHeader.StartDepot);
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
                        // TODO
                        var existingStop = this.stopRepository.GetByTransportOrderReference(stop.TransportOrderReference);

                        if (existingStop == null)
                        {
                            this.logger.LogDebug($"Existing stop not found with transport order reference {stop.TransportOrderReference}");
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
                    this.logger.LogError($"Stop has an error on Epod update! Stop Id ({stop.Id}), Transport order reference ({stop.TransportOrderReference})", exception);
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
                // TODO do we need the invoice here im thinking we dont as we have enough info already
                var existingJob = this.jobRepository.GetByAccountPicklistAndStopId(
                    job.PhAccount,
                    job.PickListRef,
                    stopId);

                if (existingJob == null)
                {
                    this.logger.LogDebug($"Existing job not found for stop id ({stopId}), Account ({job.PhAccount}), Picklist ({job.PickListRef})");
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        $"Existing job not found for stop id ({stopId}), Account ({job.PhAccount}), Picklist ({job.PickListRef})",
                        7669);

                    continue;
                }

                this.mapper.Map(job, existingJob);

                this.deliveryStatusService.SetStatus(existingJob, branchId);

                {
                    // TODO think about this status do we need it, maybe for querying hmm not sure
                } 
                // TODO refactor this
                if (!string.IsNullOrWhiteSpace(job.GrnNumberUpdate) && existingJob.GrnProcessType == 1)
                {
                    var grnEvent = new GrnEvent
                    {
                        Id = existingJob.Id,
                        BranchId = branchId
                    };

                    this.exceptionEventRepository.InsertGrnEvent(grnEvent);
                }

                //TODO POD event
                var pod = existingJob.ProofOfDelivery.GetValueOrDefault();

                if (ProofOfDeliveryList.Contains(pod) && job.IsClean)
                {
                    var podEvent = new PodEvent
                    {
                        Id = existingJob.Id,
                        BranchId = branchId
                    };

                    this.exceptionEventRepository.InsertPodEvent(podEvent);
                }
                
                this.UpdateJobDetails(job.JobDetails, existingJob.Id, string.IsNullOrWhiteSpace(existingJob.InvoiceNumber));

                this.jobRepository.Update(existingJob);
            }
        }

        private void UpdateJobDetails(IEnumerable<JobDetail> jobDetails, int jobId, bool invoiceOutstanding)
        {
            foreach (var detail in jobDetails)
            {
                var existingJobDetail = this.jobDetailRepository.GetByJobLine(jobId, detail.LineNumber);

                if (existingJobDetail == null)
                {
                    this.logger.LogDebug($"Existing job detail not found for job id ({jobId}), line number ({detail.LineNumber})");
                    continue;
                }

                this.mapper.Map(detail, existingJobDetail);

                detail.SkuGoodsValue = existingJobDetail.SkuGoodsValue;

                // TODO might need to set resolved unresolved status here and add in sub outer values
                // whole status thing im not sure about
                if (invoiceOutstanding)
                    existingJobDetail.JobDetailStatusId = (int)JobDetailStatus.AwtInvNum;

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
                damage.JobDetailReason = JobDetailReason.NotDefined;
                this.jobDetailDamageRepository.Save(damage);
            }
        }
    }
}