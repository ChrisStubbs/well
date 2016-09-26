namespace PH.Well.Api.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Repositories.Contracts;

    public class CleanPreferenceController : BaseApiController
    {
        private readonly ICleanPreferenceRepository cleanPreferenceRepository;

        public CleanPreferenceController(ICleanPreferenceRepository cleanPreferenceRepository)
        {
            this.cleanPreferenceRepository = cleanPreferenceRepository;
        }

        [Route("clean-preference")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var cleans = this.cleanPreferenceRepository.GetAll();

            return this.Request.CreateResponse(HttpStatusCode.OK, cleans);
        }
    }
}
