namespace PH.Well.Api.Controllers
{
    using System.Web.Http;
    using Domain;
    using Repositories.Contracts;

    public class SingleLocationController : ApiController
    {
        private readonly ILocationRepository locationRepository;

        public SingleLocationController(ILocationRepository locationRepository)
        {
            this.locationRepository = locationRepository;
        }

        public SingleLocation Get([FromUri]SingleLocationQuery qs)
        {
            return this.locationRepository.GetSingleLocation(qs.LocationId, qs.AccountNumber, qs.BranchId);
        }
    }

    public class SingleLocationQuery
    {
        public int? LocationId { get; set; }
        public string AccountNumber { get; set; }
        public int? BranchId { get; set; }
    }
}