namespace PH.Well.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Mapper.Contracts;
    using Models;
    using Validators.Contracts;
    using Common.Contracts;
    using Repositories.Contracts;

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
            ISeasonalDateValidator validator,
            IUserNameProvider userNameProvider)
            : base(userNameProvider)
        {
            this.seasonalDateRepository = seasonalDateRepository;
            this.logger = logger;
            this.mapper = mapper;
            this.validator = validator;

        }

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

        [Route("{branchId:int}/seasonal-date/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            this.seasonalDateRepository.Delete(id);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
        }

        [Route("{branchId:int}/seasonal-date")]
        [HttpPost]
        public HttpResponseMessage Post(SeasonalDateModel model)
        {
            if (!this.validator.IsValid(model))
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, new { notAcceptable = true, errors = this.validator.Errors.ToArray() });
            }

            var seasonalDate = this.mapper.Map(model);

            this.seasonalDateRepository.Save(seasonalDate);

            return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
        }
    }
}