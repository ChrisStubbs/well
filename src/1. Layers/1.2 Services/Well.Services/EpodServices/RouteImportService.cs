namespace PH.Well.Services.EpodServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Repositories.Contracts;

    public class RouteImportService : IAdamImportService
    {
        private readonly ILogger logger;
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IStopRepository stopRepository;
        private readonly IAccountRepository accountRepository;

        public RouteImportService(
            ILogger logger,
            IRouteHeaderRepository routeHeaderRepository,
            IStopRepository stopRepository,
            IAccountRepository accountRepository
            )
        {
            this.logger = logger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.stopRepository = stopRepository;
            this.accountRepository = accountRepository;
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

            foreach (var s in fileRouteHeader.Stops)
            {
                Stop fileStop = Mapper.Map<StopDTO, Stop>(s);

                Stop originalStop = GetOriginalStop(existingStops, fileStop);

                fileStop.Id = originalStop?.Id ?? 0;

                //Is New
                if (fileStop.IsTransient())
                {
                    stopRepository.Save(fileStop);
                    accountRepository.Save(fileStop.Account);
                }
                // Update Existing
                else if (!HasStopBeenCompleted(originalStop))
                {
                    fileStop.SetPreviously(originalStop);
                    stopRepository.Update(fileStop);
                }
            }

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

        private bool HasStopBeenCompleted(Stop stop)
        {
            return stop.WellStatus == WellStatus.Complete;
        }

        //TODO: Check this with  Fiona 
        private Stop GetOriginalStop(IList<Stop> existingStops, Stop stop)
        {
            return existingStops.FirstOrDefault(x => x.TransportOrderReference == stop.TransportOrderReference);
        }
    }
}