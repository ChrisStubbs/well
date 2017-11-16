namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Transactions;
    using Common;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using System.Linq;
    using Repositories.Contracts;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class EpodImportService : IEpodImportService
    {
        private readonly ILogger logger;
        private readonly IEventLogger eventLogger;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IImportService importService;
        private readonly IEpodImportMapper epodImportMapper;
        private readonly IEpodFileImportCommands importCommands;
        private readonly IDeadlockRetryHelper deadlockRetryHelper;
        private readonly IRouteService routeService;
        private readonly IJobService jobService;
        private readonly IStopService stopService;

        public EpodImportService(
            ILogger logger,
            IEventLogger eventLogger,
            IRouteHeaderRepository routeHeaderRepository,
            IImportService importService,
            IEpodImportMapper epodImportMapper,
            IEpodFileImportCommands importCommands,
            IDeadlockRetryHelper deadlockRetryHelper,
            IRouteService routeService,
            IJobService jobService,
            IStopService stopService)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.importService = importService;
            this.epodImportMapper = epodImportMapper;
            this.importCommands = importCommands;
            this.deadlockRetryHelper = deadlockRetryHelper;
            this.routeService = routeService;
            this.jobService = jobService;
            this.stopService = stopService;
        }
        public void Import(RouteDelivery route, string fileName, out bool hasErrors)
        {
            hasErrors = false;
            foreach (var header in route.RouteHeaders)
            {
                try
                {
                    var isRouteSuccessful = false;
                    deadlockRetryHelper.Retry(() => isRouteSuccessful = TryImportRouteHeaderTransaction(fileName, header));

                    if (!isRouteSuccessful)
                    {
                        hasErrors = true;
                    }
                }
                catch (Exception exception)
                {
                    var msg = $"Route has an error on import! Route Id ({route.RouteId})";
                    logger.LogError(msg, exception);
                    eventLogger.TryWriteToEventLog(
                        EventSource.WellEpodXmlImport,
                        msg,
                        EventId.ImportException);
                    hasErrors = true;
                }
            }
        }

        private bool TryImportRouteHeaderTransaction(string fileName, RouteHeader header)
        {
            using (var transactionScope = new TransactionScope())
            {
                if (TryImportRouteHeader(header, fileName))
                {
                    var allJobs = header.Stops
                        .SelectMany(p => p.Jobs)
                        .Where(p => p.Id != 0)
                        .Select(p => new Job { Id = p.Id, JobStatus = p.JobStatus, StopId = p.StopId })
                        .ToList();
                    var allStops = new List<Stop>();

                    foreach (var j in allJobs)
                    {
                        this.jobService.ComputeWellStatus(j);
                    }

                    allStops = allJobs
                        .GroupBy(p => p.StopId)
                        .Select(p => new Stop
                        {
                            Id = p.Key,
                            Jobs = p.ToList()
                        })
                        .ToList();

                    stopService.ComputeWellStatus(allStops);
                    this.routeService.ComputeWellStatus(header.Id);

                    transactionScope.Complete();
                    return true;
                }

                transactionScope.Complete();
                return false;
            }
        }

        public bool TryImportRouteHeader(RouteHeader fileHeader, string fileName)
        {
            int branchId;

            if (fileHeader.TryParseBranchIdFromRouteNumber(out branchId))
            {
                var existingHeader = routeHeaderRepository.GetRouteHeaderByRoute(
                    branchId,
                    fileHeader.RouteNumber.Substring(2),
                    fileHeader.RouteDate);
                if (existingHeader == null)
                {
                    var message = $"RouteDelivery Ignored could not find matching RouteHeader," +
                                  $"Branch: {branchId} " +
                                  $"RouteNumber: {fileHeader.RouteNumber.Substring(2)} " +
                                  $"RouteDate: {fileHeader.RouteDate} " +
                                  $"FileName: {fileName}";

                    logger.LogDebug(message);

                    eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, message, EventId.EpodUpdateIgnored);

                    return false;
                }

                epodImportMapper.MergeRouteHeader(fileHeader, existingHeader);
                routeHeaderRepository.Update(existingHeader);

                fileHeader.Id = existingHeader.Id;

                importService.ImportStops(fileHeader, epodImportMapper, importCommands);

                // Calculate well status
                //routeService.ComputeWellStatusAndNotifyIfChangedFromCompleted(existingHeader.Id);
                return true;
            }
            else
            {
                var message = $" Route Number Depot Indicator is not an int... Route Number Depot passed in from from transend is ({fileHeader.RouteNumber}) file {fileName}";
                logger.LogDebug(message);
                eventLogger.TryWriteToEventLog(EventSource.WellEpodXmlImport, message, EventId.ImportIgnored);
                return false;
            }
        }
    }
}
