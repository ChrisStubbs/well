namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Api.Validators.Contracts;
    using PH.Well.Common.Contracts;
    using PH.Well.Repositories.Contracts;

    public class CleanPreferenceController : BaseApiController
    {
        private readonly ICleanPreferenceRepository cleanPreferenceRepository;

        private readonly ICleanPreferenceMapper mapper;

        private readonly ILogger logger;

        private readonly ICleanPreferenceValidator validator;

        public CleanPreferenceController(
            ICleanPreferenceRepository cleanPreferenceRepository,
            ICleanPreferenceMapper mapper, 
            ILogger logger,
            ICleanPreferenceValidator validator)
        {
            this.cleanPreferenceRepository = cleanPreferenceRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.validator = validator;
            this.cleanPreferenceRepository.CurrentUser = this.UserIdentityName;
        }

        [Route("clean-preference")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var cleans = this.cleanPreferenceRepository.GetAll().ToList();

            var model = new List<CleanPreferenceModel>();

            foreach (var clean in cleans)
            {
                var mappedModel = this.mapper.Map(clean);

                model.Add(mappedModel);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [Route("clean-preference")]
        [HttpPost]
        public HttpResponseMessage Post(CleanPreferenceModel model)
        {
            try
            {
                if (!this.validator.IsValid(model))
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { notAcceptable = true, errors = this.validator.Errors.ToArray() });
                }
                
                var cleanPreference = this.mapper.Map(model);

                this.cleanPreferenceRepository.Save(cleanPreference);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when trying to save clean preference", exception);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { error = true });
            }
        }

        [Route("clean-preference/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                this.cleanPreferenceRepository.Delete(id);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError($"Error when trying to delete clean preference (id):{id}", exception);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { error = true });
            }
        }
    }
}
