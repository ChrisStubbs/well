namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Transactions;
    using Common;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Repositories.Contracts;

    public class EpodImportService
    {
        private readonly ILogger logger;
        private readonly IEventLogger eventLogger;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IRouteMapper routeMapper;
        private readonly IImportService importService;

        public EpodImportService(
            ILogger logger,
            IEventLogger eventLogger,
            IRouteHeaderRepository routeHeaderRepository,
            IRouteMapper routeMapper,
            IImportService importService
            )
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.routeMapper = routeMapper;
            this.importService = importService;
        }
        public void Import(RouteDelivery route, string fileName)
        {
            foreach (var header in route.RouteHeaders)
            {
                try
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        ImportRouteHeader(header, fileName);
                        transactionScope.Complete();
                    }
                }
                catch (Exception exception)
                {
                    string msg = $"Route has an error on import! Route Id ({route.RouteId})";
                    this.logger.LogError(msg, exception);
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellEpodXmlImport,
                        msg,
                        EventId.ImportException);
                }
            }
        }

        public void ImportRouteHeader(RouteHeader header, string fileName)
        {
            int branchId;

            if (header.TryParseBranchIdFromRouteNumber(out branchId))
            {
                var existingHeader = this.routeHeaderRepository.GetRouteHeaderByRoute(
                    branchId,
                    header.RouteNumber.Substring(2),
                    header.RouteDate);
                if (existingHeader == null)
                {
                    var message = $"RouteDelivery Ignored could not find matching RouteHeader," +
                                  $"Branch: {branchId} " +
                                  $"RouteNumber: {header.RouteNumber.Substring(2)} " +
                                  $"RouteDate: {header.RouteDate} " +
                                  $"FileName: {fileName}";

                    logger.LogDebug(message);

                    eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, message, EventId.EpodUpdateIgnored);

                    return;
                }

                this.routeMapper.Map(header, existingHeader);
                this.routeHeaderRepository.Update(existingHeader);
                importService.ImportStops(header);
            }
            else
            {
                var message = $" Route Number Depot Indicator is not an int... Route Number Depot passed in from from transend is ({header.RouteNumber}) file {fileName}";
                this.logger.LogDebug(message);
                this.eventLogger.TryWriteToEventLog(EventSource.WellEpodXmlImport, message, EventId.ImportIgnored);
            }
        }
    }
}
