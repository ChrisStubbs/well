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
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    public class SeasonalDateController : BaseApiController
    {
        private readonly ISeasonalDateRepository seasonalDateRepository;

        private readonly ILogger logger;

        private readonly ISeasonalDateMapper mapper;

        private readonly ISeasonalDateValidator validator;

        public SeasonalDateController(
            ISeasonalDateRepository seasonalDateRepository, 
            ILogger logger, 
            ISeasonalDateMapper mapper,
            ISeasonalDateValidator validator)
        {
            this.seasonalDateRepository = seasonalDateRepository;
            this.logger = logger;
            this.mapper = mapper;
            this.validator = validator;

            this.seasonalDateRepository.CurrentUser = this.UserIdentityName;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            var seasonalDates = this.seasonalDateRepository.GetAll().OrderBy(x => x.From).ToList();

            var model = new List<SeasonalDateModel>();

            foreach (var seasonalDate in seasonalDates)
            {
                var mappedModel = this.mapper.Map(seasonalDate);

                model.Add(mappedModel);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [Route("seasonal-date/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                this.seasonalDateRepository.Delete(id);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError($"Error when trying to delete seasonal date (id):{id}", exception);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { failure = true });
            }
        }

        [Route("seasonal-date")]
        [HttpPost]
        public HttpResponseMessage Post(SeasonalDateModel model)
        {
            try
            {
                if (!this.validator.IsValid(model))
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { notAcceptable = true, errors = this.validator.Errors.ToArray() });
                }

                var seasonalDate = this.mapper.Map(model);

                this.seasonalDateRepository.Save(seasonalDate);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError($"Error when trying to save seasonal date {model.Description}", exception);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { failure = true });
            }
        }
    }
}