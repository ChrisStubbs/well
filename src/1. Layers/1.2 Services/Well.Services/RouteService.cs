using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IStopRepository stopRepository;

        #endregion Private fields

        #region Constructors
        public RouteService(IRouteHeaderRepository routeHeaderRepository, IWellStatusAggregator wellStatusAggregator,IStopRepository stopRepository)
        {
            this.routeHeaderRepository = routeHeaderRepository;
            this.wellStatusAggregator = wellStatusAggregator;
            this.stopRepository = stopRepository;
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
            throw new ArgumentException($"RouteHeader not found id : {routeId}", nameof(routeId));
        }

        public bool ComputeWellStatus(RouteHeader route)
        {
            // Compute new route status from all its active stops
            WellStatus[] wellStatuses;
            if (!route.Stops.Any())
            {
                wellStatuses = stopRepository.GetStopByRouteHeaderId(route.Id).Select(x => x.WellStatus).ToArray();
            }
            else
            {
                wellStatuses = route.Stops.Select(x => x.WellStatus).ToArray();
            }

            var newWellStatus = wellStatusAggregator.Aggregate(wellStatuses.ToArray());

            if (route.RouteWellStatus != newWellStatus)
            {
                // Save the status change back to the repository
                route.RouteWellStatus = newWellStatus;
                routeHeaderRepository.UpdateWellStatus(route);
                return true;
            }

            return false;
        }

        #endregion Public methods
    }
}
