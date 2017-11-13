using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain;
using PH.Well.Domain.Enums;
using PH.Well.Repositories.Contracts;
using PH.Well.Services.Contracts;

namespace PH.Well.Services
{
    public class StopService : IStopService
    {
        private readonly IStopRepository stopRepository;
        private readonly IRouteService routeService;
        private readonly IWellStatusAggregator wellStatusAggregator;
        private readonly IJobRepository jobRepository;

        public StopService(IStopRepository stopRepository, IRouteService routeService, IWellStatusAggregator wellStatusAggregator,IJobRepository jobRepository)
        {
            this.routeService = routeService;
            this.wellStatusAggregator = wellStatusAggregator;
            this.jobRepository = jobRepository;
            this.stopRepository = stopRepository;
        }
        public void ComputeWellStatus(IList<int> stopId)
        {
            var stops = new ConcurrentBag<Stop>();

            Parallel.ForEach(stopRepository.GetForWellStatusCalculationById(stopId), s =>
            {
                if (ComputeWellStatus(s))
                {
                    stops.Add(s);
                }
            });

            this.SaveNewWellStatus(stops.Select(p => p).ToList());
        }

        public void ComputeWellStatus(IList<Stop> stops)
        {
            var all = new ConcurrentBag<Stop>();
            
            Parallel.ForEach(stops, s =>
            {
                if (ComputeWellStatus(s))
                {
                    all.Add(s);
                }
            });

            this.SaveNewWellStatus(all.Select(p => p).ToList());
        }

        public bool ComputeAndPropagateWellStatus(Stop stop)
        {
            if (ComputeWellStatus(stop))
            {
                this.SaveNewWellStatus(new[] { stop }.ToList());

                // Cascade any change back up to the route header
                this.routeService.ComputeWellStatus(stop.RouteHeaderId);

                return true;
            }

            return false;
        }

        public bool ComputeAndPropagateWellStatus(int stopId)
        {
            var stop = stopRepository.GetForWellStatusCalculationById(stopId);

            if (stop == null)
            {
                throw new ArgumentException($"Stop not found id : {stopId}", nameof(stopId));
            }

            return ComputeAndPropagateWellStatus(stop);
        }

        private void SaveNewWellStatus(IList<Stop> stops)
        {
            if (stops.Any())
            {
                stopRepository.UpdateWellStatus(stops);
            }
        }

        private bool ComputeWellStatus(Stop stop)
        {
            // Compute new well status
            var newWellStatus = wellStatusAggregator.Aggregate(stop.Jobs.Select(x => x.WellStatus).ToArray());

            if (stop.WellStatus != newWellStatus)
            {
                stop.WellStatus = newWellStatus;

                return true;
            }

            return false;
        }
    }
}
