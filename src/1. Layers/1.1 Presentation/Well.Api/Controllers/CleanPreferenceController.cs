namespace PH.Well.Api.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Repositories.Contracts;

    public class CleanPreferenceController : BaseApiController
    {
        private readonly ICleanPreferenceRepository cleanPreferenceRepository;

        private readonly ICleanPreferenceMapper mapper;

        public CleanPreferenceController(ICleanPreferenceRepository cleanPreferenceRepository, ICleanPreferenceMapper mapper)
        {
            this.cleanPreferenceRepository = cleanPreferenceRepository;
            this.mapper = mapper;
        }

        [Route("clean-preference")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var cleans = this.cleanPreferenceRepository.GetAll();

            var model = new List<CleanPreferenceModel>();

            foreach (var clean in cleans)
            {
                var mappedModel = this.mapper.Map(clean);

                model.Add(mappedModel);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, model);
        }
    }
}
