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
        private readonly IJobRepository jobRepository;

        #endregion Private fields

        #region Constructors
        public StopService(IStopRepository stopRepository, IRouteService routeService, IWellStatusAggregator wellStatusAggregator,IJobRepository jobRepository)
        {
            this.routeService = routeService;
            this.wellStatusAggregator = wellStatusAggregator;
            this.jobRepository = jobRepository;
            this.stopRepository = stopRepository;
        }
        #endregion Constructors

        #region Public methods
        
        public bool ComputeWellStatus(int stopId)
        {
            var stop = GetStopForWellStatusCalculation(stopId);
            return ComputeWellStatus(stop);
        }

        public bool ComputeWellStatus(Stop stop)
        {
            // Compute new well status
            var newWellStatus = wellStatusAggregator.Aggregate(stop.Jobs.Select(x => x.WellStatus).ToArray());

            if (stop.WellStatus != newWellStatus)
            {
                stop.WellStatus = newWellStatus;
                stopRepository.UpdateWellStatus(stop);
                return true;
            }

            return false;
        }

        public bool ComputeAndPropagateWellStatus(int stopId)
        {
            var stop = GetStopForWellStatusCalculation(stopId);
            return ComputeAndPropagateWellStatus(stop);
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

        #region Private methods

        /// <summary>
        /// Helper method to get stop with minimum information required for computing WellStatus
        /// </summary>
        /// <param name="stopId"></param>
        /// <exception cref="ArgumentException">When stop not found</exception>
        /// <returns></returns>
        private Stop GetStopForWellStatusCalculation(int stopId)
        {
            var stop = stopRepository.GetForWellStatusCalculationById(stopId);
            if (stop != null)
            {
                return stop;
            }
            throw new ArgumentException($"Stop not found id : {stopId}", nameof(stopId));
        }

        #endregion  Private methods
    }
}
