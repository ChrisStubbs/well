namespace PH.Well.Api.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Repositories.Contracts;

    public class SeasonalDateController : BaseApiController
    {
        private readonly ISeasonalDateRepository seasonalDateRepository;

        public SeasonalDateController(ISeasonalDateRepository seasonalDateRepository)
        {
            this.seasonalDateRepository = seasonalDateRepository;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            var seasonalDates = this.seasonalDateRepository.GetAll().OrderBy(x => x.From).ToList();

            return this.Request.CreateResponse(HttpStatusCode.OK, seasonalDates);
        }
    }
}