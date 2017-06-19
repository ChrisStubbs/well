namespace PH.Well.Api.Controllers
{
    using System.Web.Http;
    using Domain;
    using Repositories.Contracts;

    public class LocationController : ApiController
    {
        private readonly ILocationRepository locationRepository;

        public LocationController(ILocationRepository locationRepository)
        {
            this.locationRepository = locationRepository;
        }

        public Location GetById(int locationId)
        {
            return this.locationRepository.GetLocationById(locationId);
        }
    }
}