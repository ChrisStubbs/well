using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain;
using PH.Well.Domain.Enums;
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
                return ComputeWellStatus(routeHeader);
            }
            return false;
        }

        public bool ComputeWellStatus(RouteHeader route)
        {
            // Compute new route status from all its active stops
            var newWellStatus = wellStatusAggregator.Aggregate(AggregationType.Route,
                route.Stops.Select(x => x.WellStatus).ToArray());

            if (route.RouteWellStatus != newWellStatus)
            {
                // Save the status change back to the repository
                route.RouteWellStatus = newWellStatus;
                routeHeaderRepository.Update(route);
                return true;
            }

            return false;
        }

        #endregion Public methods
    }
}
