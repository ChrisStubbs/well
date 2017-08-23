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
    public class LocationService : ILocationService
    {
        #region Private fields
        private readonly ILocationRepository locationRepository;
        private readonly IWellStatusAggregator wellStatusAggregator;
        #endregion Private fields

        #region Constructors
        public LocationService(ILocationRepository locationRepository, IWellStatusAggregator wellStatusAggregator)
        {
            this.locationRepository = locationRepository;
            this.wellStatusAggregator = wellStatusAggregator;
        }
        #endregion Constructors

        #region Public methods
        public bool ComputeWellStatus(int locationId)
        {
            // TODO: DIJ - We will not aggregate/propagate location statuses for now
            //var location = this.locationRepository.GetSingleLocationById(locationId);
            //if (location != null)
            //{
            //    //WellStatus status = location.WellStatus;
            //    //wellStatusAggregator.Aggregate(location.)
            //    return true;
            //}
            return false;
        }
        #endregion Public methods
    }
}
