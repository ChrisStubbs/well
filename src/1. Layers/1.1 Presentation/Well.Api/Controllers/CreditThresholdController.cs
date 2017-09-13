using PH.Well.Services.Contracts;

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

    //[PHAuthorize(Permissions = Consts.Security.PermissionWellAdmin)]
    public class CreditThresholdController : BaseApiController
    {
        private readonly ICreditThresholdRepository creditThresholdRepository;
        private readonly ILogger logger;
        private readonly ICreditThresholdMapper mapper;
        private readonly IUserRepository userRepository;
        private readonly IUserThresholdService userThresholdService;

        public CreditThresholdController(
            ICreditThresholdRepository creditThresholdRepository, 
            ILogger logger, 
            ICreditThresholdMapper mapper,
            IUserRepository userRepository,
            IUserNameProvider userNameProvider,
            IUserThresholdService userThresholdService)
            : base(userNameProvider)
        {
            this.creditThresholdRepository = creditThresholdRepository;
            this.logger = logger;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.userThresholdService = userThresholdService;
        }

        [Route("credit-threshold")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var creditThresholds = this.creditThresholdRepository.GetAll().ToList();

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
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            try
            {
                var creditThreshold = this.mapper.Map(model);

                if (isUpdate)
                {
                    creditThresholdRepository.Update(creditThreshold);
                }
                else
                {
                    creditThresholdRepository.Save(creditThreshold);
                }
                
                return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when trying to save credit threshold date", exception);
                throw;
            }
        }

        [Route("credit-threshold/getByUser")]
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage GetByUser()
        {
            try
            {
                var threshold = userThresholdService.GetCreditThreshold(this.UserIdentityName);
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