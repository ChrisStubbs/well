namespace PH.Well.Api.Controllers
{
    using System;
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
    using Validators;

    [PHAuthorize(Permissions = Consts.Security.PermissionWellAdmin)]
    public class CreditThresholdController : BaseApiController
    {
        private readonly ICreditThresholdRepository creditThresholdRepository;

        private readonly ILogger logger;

        private readonly ICreditThresholdMapper mapper;

        private readonly ICreditThresholdValidator validator;

        private readonly IUserRepository userRepository;

        public CreditThresholdController(
            ICreditThresholdRepository creditThresholdRepository, 
            ILogger logger, 
            ICreditThresholdMapper mapper,
            ICreditThresholdValidator validator,
            IUserRepository userRepository)
        {
            this.creditThresholdRepository = creditThresholdRepository;
            this.logger = logger;
            this.mapper = mapper;
            this.validator = validator;
            this.userRepository = userRepository;

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

        [Route("credit-threshold/getByUser")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetByUser()
        {
            try
            {
                var threshold = this.userRepository.GetCreditThresholds(this.UserIdentityName);

                return this.Request.CreateResponse(HttpStatusCode.OK, threshold);
            }
            catch (Exception exception)
            {
                this.logger.LogError($"Error when trying to get credit threshold for:{this.UserIdentityName}", exception);

                return this.Request.CreateResponse(HttpStatusCode.OK, new { failure = true });
            }
        }
    }
}