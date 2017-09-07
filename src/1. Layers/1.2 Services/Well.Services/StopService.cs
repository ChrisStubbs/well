using System;
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
        #region Private fields
        private readonly IStopRepository stopRepository;
        private readonly IRouteService routeService;
        private readonly IWellStatusAggregator wellStatusAggregator;
        #endregion Private fields

        #region Constructors
        public StopService(IStopRepository stopRepository, IRouteService routeService, IWellStatusAggregator wellStatusAggregator)
        {
            this.routeService = routeService;
            this.wellStatusAggregator = wellStatusAggregator;
            this.stopRepository = stopRepository;
        }
        #endregion Constructors

        #region Public methods
        public bool ComputeWellStatus(int stopId)
        {
            var stop = stopRepository.GetById(stopId);
            if (stop != null)
            {
                return ComputeWellStatus(stop);
            }
            return false;
        }

        public bool ComputeWellStatus(Stop stop)
        {
            // Compute new well status
            var newWellStatus = wellStatusAggregator.Aggregate(AggregationType.Stop,
                stop.Jobs.Select(x => x.WellStatus).ToArray());

            if (stop.WellStatus != newWellStatus)
            {
                stop.WellStatus = newWellStatus;
                stopRepository.Update(stop);
                return true;
            }

            return false;
        }

        public bool ComputeAndPropagateWellStatus(int stopId)
        {
            var stop = stopRepository.GetById(stopId);
            if (stop != null)
            {
                return ComputeAndPropagateWellStatus(stop);
            }
            return false;
        }

        public bool ComputeAndPropagateWellStatus(Stop stop)
        {
            var changed = ComputeWellStatus(stop);
            if (changed)
            {
                // Cascade any change back up to the route header
                this.routeService.ComputeWellStatus(stop.RouteHeaderId);
            }

            return changed;
        }

        #endregion Public methods
    }
}
