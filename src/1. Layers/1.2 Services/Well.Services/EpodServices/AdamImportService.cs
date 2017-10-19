﻿namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Transactions;
    using Common;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Repositories.Contracts;
    using System.Linq;
    using System.Diagnostics;

    public class AdamImportService : IAdamImportService
    {
        private readonly IEventLogger eventLogger;
        private readonly ILogger logger;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IImportService importService;
        private readonly IAdamImportMapper importMapper;
        private readonly IAdamFileImportCommands importCommands;
        private readonly IDeadlockRetryHelper deadlockRetryHelper;
        private readonly IDbConfiguration dbConfiguration;
        private readonly IRouteService routeService;

        public AdamImportService(
            ILogger logger,
            IEventLogger eventLogger,
            IRouteHeaderRepository routeHeaderRepository,
            IImportService importService,
            IAdamImportMapper importMapper,
            IAdamFileImportCommands importCommands,
            IDeadlockRetryHelper deadlockRetryHelper,
            IDbConfiguration dbConfiguration,
            IRouteService routeService
            )
        {
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.importService = importService;
            this.importMapper = importMapper;
            this.importCommands = importCommands;
            this.deadlockRetryHelper = deadlockRetryHelper;
            this.dbConfiguration = dbConfiguration;
            this.routeService = routeService;
        }

        public void Import(RouteDelivery route, string fileName,IImportConfig config)
        {
            var existingRouteHeader = this.routeHeaderRepository.GetByNumberDateBranch(route.RouteHeaders
                .Select(p =>
                {
                    p.RouteOwnerId = GetBranchId(p, fileName);

                    return new GetByNumberDateBranchFilter
                    {
                        BranchId = p.RouteOwnerId,
                        RouteDate = p.RouteDate.Value,
                        RouteNumber = p.RouteNumber
                    };
                })
                .ToList())
                .ToDictionary(k => new { k.BranchId, k.RouteDate, k.RouteNumber });

            foreach (var header in route.RouteHeaders)
            {
                try
                {
                    header.RouteOwnerId = GetBranchId(header, fileName);

                    if (!config.ProcessDataForBranch((Domain.Enums.Branch) header.RouteOwnerId))
                    {
                        logger.LogDebug($"Skip route header {fileName}");
                        continue;
                    }

                    using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(dbConfiguration.TransactionTimeout)))
                    {
                        var key = new
                        {
                            BranchId = header.RouteOwnerId,
                            RouteDate = header.RouteDate.Value,
                            header.RouteNumber
                        };

                        deadlockRetryHelper.Retry(() =>
                            this.ImportRouteHeader(header,
                                route.RouteId,
                                existingRouteHeader.ContainsKey(key) ? existingRouteHeader[key] : null)
                        );

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

            routeHeaderRepository.DeleteRouteHeaderWithNoStops();
        }

        private void ImportRouteHeader(RouteHeader header, int routeId, GetByNumberDateBranchResult existingRouteHeader = null)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            header.RoutesId = routeId;

            if (existingRouteHeader != null)
            {
                if (existingRouteHeader.WellStatus == WellStatus.Complete)
                {
                    var message = $"Ignoring Route update. Route is Complete  " +
                                  $"route header id ({existingRouteHeader.Id}) " +
                                  $"number ({header.RouteNumber}), " +
                                  $"route date ({header.RouteDate.Value}), " +
                                  $"branch ({header.RouteOwnerId})";
                    logger.LogDebug(message);
                    this.eventLogger.TryWriteToEventLog(EventSource.WellAdamXmlImport, message, EventId.ImportIgnored);
                    return;
                }

                header.Id = existingRouteHeader.Id;
                routeHeaderRepository.UpdateFieldsFromImported(importMapper.MapRouteHeader(header));

                logger.LogDebug(
                    $"Updating Route  " +
                    $"route header id ({existingRouteHeader.Id}) " +
                    $"number ({header.RouteNumber}), " +
                    $"route date ({header.RouteDate.Value}), " +
                    $"branch ({header.RouteOwnerId})"
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

            // Calculate well status
            routeService.ComputeWellStatusAndNotifyIfChangedFromCompleted(header.Id);

            var ts = stopWatch.Elapsed;

            var elapsedTime = $"route header id ({header.Id}) took {ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";

            logger.LogDebug(elapsedTime);
        }

        public virtual int GetBranchId(RouteHeader header, string filename)
        {
            var branchId = GetBranchId(header.RouteOwner);
            if (branchId == (int)Branches.NotDefined)
            {
                branchId = GetBranchIdFallBack(filename);
            }
            return branchId;
        }

        private int GetBranchIdFallBack(string filename)
        {
            var split = filename.Split('_');
            if (split.Length > 1)
            {
                return GetBranchId(split[1]);
            }
            return (int)Branches.NotDefined;
        }

        public virtual int GetBranchId(string branchShortName)
        {
            return string.IsNullOrWhiteSpace(branchShortName)
                ? (int)Branches.NotDefined
                : (int)Enum.Parse(typeof(Branches), branchShortName, true);
        }
    }
}