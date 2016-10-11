using PH.Well.Domain;
using PH.Well.Repositories;
using PH.Well.Repositories.Contracts;

namespace PH.Well.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Services;
    using PH.Well.Services.Contracts;

    public class ExceptionEventController : BaseApiController
    {
        private readonly ILogger logger;
        private readonly IExceptionEventService exceptionEventService;
        private readonly IUserThresholdService userThresholdService;

        public ExceptionEventController(ILogger logger, 
            IExceptionEventService exceptionEventService,
            IUserThresholdService userThresholdService)
        {
            this.logger = logger;
            this.exceptionEventService = exceptionEventService;
            this.userThresholdService = userThresholdService;
        }

        [Route("credit")]
        [HttpPost]
        public HttpResponseMessage Credit(CreditEvent creditEvent)
        {
            try
            {
                var canCredit = this.userThresholdService.CanUserCredit(this.UserIdentityName, creditEvent.TotalCreditValueForThreshold);

                if (canCredit)
                {
                    var settings = AdamSettingsFactory.GetAdamSettings((Branch)creditEvent.BranchId);

                    var response = this.exceptionEventService.Credit(creditEvent, settings, this.UserIdentityName);

                    if (response == AdamResponse.AdamDown) return this.Request.CreateResponse(HttpStatusCode.OK, new { adamdown = true });

                    return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error on credit event", exception);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}