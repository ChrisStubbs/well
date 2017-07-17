using System.Collections.Generic;
using System.Web.Http;
using PH.Well.Domain;
using PH.Well.Repositories.Contracts;

namespace PH.Well.Api.Controllers
{
    public class LocationController : ApiController
    {
        private readonly ILocationRepository locationRepository;

        public LocationController(ILocationRepository locationRepository)
        {
            this.locationRepository = locationRepository;
        }

        public IList<Location> Get(int id)
        {
            return locationRepository.GetLocation(id);
        }
    }
}
