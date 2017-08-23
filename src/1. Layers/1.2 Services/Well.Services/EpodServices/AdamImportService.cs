namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Transactions;
    using Common;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Repositories.Contracts;

    public class AdamImportService : IAdamImportService
    {
        private readonly IEventLogger eventLogger;
        private readonly ILogger logger;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IImportService importService;
        private readonly IAdamImportMapper importMapper;
        private readonly IAdamFileImportCommands importCommands;

        public AdamImportService(
            ILogger logger,
            IEventLogger eventLogger,
            IRouteHeaderRepository routeHeaderRepository,
            IImportService importService,
            IAdamImportMapper importMapper,
            IAdamFileImportCommands importCommands)
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.importService = importService;
            this.importMapper = importMapper;
            this.importCommands = importCommands;
        }

        public void Import(RouteDelivery route)
        {
            foreach (var header in route.RouteHeaders)
            {
                try
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        this.ImportRouteHeader(header, route.RouteId);
                        transactionScope.Complete();
                    }
                }
                catch (Exception exception)
                {
                    string msg = $"Route has an error on import! Route Id ({route.RouteId})";
                    this.logger.LogError(msg, exception);
                    this.eventLogger.TryWriteToEventLog(
                        EventSource.WellAdamXmlImport,
                        msg,
                        EventId.ImportException);
                }
            }
        }

        public void ImportRouteHeader(RouteHeader header, int routeId)
        {
            header.RoutesId = routeId;
            header.RouteOwnerId = GetRouteOwnerId(header);

            var existingRouteHeader = this.routeHeaderRepository.GetByNumberDateBranch(
                header.RouteNumber,
                header.RouteDate.Value,
                header.RouteOwnerId);

            if (existingRouteHeader != null)
            {
                if (existingRouteHeader.IsCompleted)
                {
                    var message = $"Ignoring Route update. Route is Complete  " +
                                  $"route header id ({existingRouteHeader.Id}) " +
                                  $"number ({existingRouteHeader.RouteNumber}), " +
                                  $"route date ({existingRouteHeader.RouteDate.Value}), " +
                                  $"branch ({existingRouteHeader.RouteOwnerId})";
                    logger.LogDebug(message);
                    this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, message, EventId.ImportIgnored);
                    return;
                }

                header.Id = existingRouteHeader.Id;
                existingRouteHeader = importMapper.MapRouteHeader(header, existingRouteHeader);

                routeHeaderRepository.Update(existingRouteHeader);
                logger.LogDebug(
                    $"Updating Route  " +
                    $"route header id ({existingRouteHeader.Id}) " +
                    $"number ({existingRouteHeader.RouteNumber}), " +
                    $"route date ({existingRouteHeader.RouteDate.Value}), " +
                    $"branch ({existingRouteHeader.RouteOwnerId})"
                );
            }
            else
            {
                header.RouteStatusDescription = "Not Started";
                routeHeaderRepository.Save(header);

                logger.LogDebug(
                    $"Inserting Route  " +
                    $"route header id ({header.Id}) " +
                    $"number ({header.RouteNumber}), " +
                    $"route date ({header.RouteDate.Value}), " +
                    $"branch ({header.RouteOwnerId})"
                );
            }

            importService.ImportStops(header, importMapper, importCommands);
        }

        public virtual int GetRouteOwnerId(RouteHeader header)
        {
            return string.IsNullOrWhiteSpace(header.RouteOwner)
                 ? (int)Branches.NotDefined
                 : (int)Enum.Parse(typeof(Branches), header.RouteOwner, true);
        }


    }
}