namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Api.Models;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;

    public class CreditThresholdController : BaseApiController
    {
        private readonly ICreditThresholdRepository creditThresholdRepository;

        private readonly ILogger logger;

        private readonly ICreditThresholdMapper mapper;

        public CreditThresholdController(ICreditThresholdRepository creditThresholdRepository, ILogger logger, ICreditThresholdMapper mapper)
        {
            this.creditThresholdRepository = creditThresholdRepository;
            this.logger = logger;
            this.mapper = mapper;

            this.creditThresholdRepository.CurrentUser = this.UserIdentityName;
        }

        [Route("credit-threshold")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var creditThresholds = this.creditThresholdRepository.GetAll();

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

                return this.Request.CreateResponse(HttpStatusCode.OK, new { error = true });
            }
        }

        [Route("credit-threshold")]
        [HttpPost]
        public HttpResponseMessage Post(CreditThresholdModel model)
        {
            try
            {
                // TODO Validate
                // if validation fails pass back 
                // return this.Request.CreateResponse(HttpStatusCode.OK, new { warning = true, message = 'validation messages' });
                
                var creditThreshold = this.mapper.Map(model);

                this.creditThresholdRepository.Save(creditThreshold);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when trying to save credit threshold date", exception);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { error = true });
            }
        }
    }
}