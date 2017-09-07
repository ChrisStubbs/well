using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain;
using PH.Well.Domain.ValueObjects;
using PH.Well.Repositories.Contracts;
using PH.Well.Services.Contracts;

namespace PH.Well.Services
{
    public class RouteService : IRouteService
    {
        #region Private fields
        private readonly IRouteHeaderRepository routeHeaderRepository;
        private readonly IWellStatusAggregator wellStatusAggregator;

        #endregion Private fields

        #region Constructors
        public RouteService(IRouteHeaderRepository routeHeaderRepository, IWellStatusAggregator wellStatusAggregator)
        {
            this.routeHeaderRepository = routeHeaderRepository;
            this.wellStatusAggregator = wellStatusAggregator;
        }
        #endregion Constructors

        #region Public methods
        public bool ComputeWellStatus(int routeId)
        {
            RouteHeader routeHeader = routeHeaderRepository.GetRouteHeaderById(routeId);
            if (routeHeader != null)
            {
                var newWellStatus = routeHeader.RouteWellStatus;

                // Compute new route status from all its active stops
                newWellStatus = wellStatusAggregator.Aggregate(AggregationType.Route,
                    routeHeader.Stops.Select(x => x.WellStatus).ToArray());

                if (routeHeader.RouteWellStatus != newWellStatus)
                {
                    // Save the status change back to the repository
                    routeHeader.RouteWellStatus = newWellStatus;
                    routeHeaderRepository.Save(routeHeader);
                    return true;
                }
            }
            return false;
        }
        #endregion Public methods
    }
}
