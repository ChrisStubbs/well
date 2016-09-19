namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    public class SeasonalDateController : BaseApiController
    {
        private readonly ISeasonalDateRepository seasonalDateRepository;

        private readonly ILogger logger;

        public SeasonalDateController(ISeasonalDateRepository seasonalDateRepository, ILogger logger)
        {
            this.seasonalDateRepository = seasonalDateRepository;
            this.logger = logger;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            var seasonalDates = this.seasonalDateRepository.GetAll().OrderBy(x => x.From).ToList();

            return this.Request.CreateResponse(HttpStatusCode.OK, seasonalDates);
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

                return this.Request.CreateResponse(HttpStatusCode.OK, new { error = true });
            }
        }

        [Route("seasonal-date")]
        [HttpPost]
        public HttpResponseMessage Post(SeasonalDate seasonalDate)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
        }
    }
}