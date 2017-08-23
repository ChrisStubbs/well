using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                // Compute the overall status of the child jobs in this stop
                WellStatus newWellStatus = stop.WellStatus;

                // Compute new well status
                newWellStatus = wellStatusAggregator.Aggregate(stop.Jobs.Select(x => x.WellStatus),
                    AggregationType.Stop);

                if (stop.WellStatus != newWellStatus)
                {
                    stop.WellStatus = newWellStatus;
                    stopRepository.Save(stop);
                    // Cascade any change back up to the route header
                    this.routeService.ComputeWellStatus(stop.RouteHeaderId);
                    return true;
                }
            }
            return false;
        }
        #endregion Public methods
    }
}
