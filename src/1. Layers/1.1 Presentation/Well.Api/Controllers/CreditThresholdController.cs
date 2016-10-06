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

    public class CreditThresholdController : BaseApiController
    {
        private readonly ICreditThresholdRepository creditThresholdRepository;

        private readonly ILogger logger;

        private readonly ICreditThresholdMapper mapper;

        private readonly ICreditThresholdValidator validator;

        public CreditThresholdController(
            ICreditThresholdRepository creditThresholdRepository, 
            ILogger logger, 
            ICreditThresholdMapper mapper,
            ICreditThresholdValidator validator)
        {
            this.creditThresholdRepository = creditThresholdRepository;
            this.logger = logger;
            this.mapper = mapper;
            this.validator = validator;

            this.creditThresholdRepository.CurrentUser = this.UserIdentityName;
        }

        [Route("credit-threshold")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var creditThresholds = this.creditThresholdRepository.GetAll().OrderBy(x => x.ThresholdLevelId).ToList();

            var model = new List<CreditThresholdModel>();

            foreach (var creditThreshold in creditThresholds)
            {
                var mappedModel = this.mapper.Map(creditThreshold);

                model.Add(mappedModel);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [Route("credit-threshold/{id:int}")]
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                this.creditThresholdRepository.Delete(id);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError($"Error when trying to delete credit threshold (id):{id}", exception);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { failure = true });
            }
        }

        [Route("credit-threshold/{isUpdate:bool}")]
        [HttpPost]
        public HttpResponseMessage Post(CreditThresholdModel model, bool isUpdate)
        {
            try
            {
                if (!this.validator.IsValid(model, isUpdate))
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { notAcceptable = true, errors = this.validator.Errors.ToArray() });
                } 
                
                var creditThreshold = this.mapper.Map(model);

                this.creditThresholdRepository.Save(creditThreshold);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when trying to save credit threshold date", exception);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { failure = true });
            }
        }
    }
}