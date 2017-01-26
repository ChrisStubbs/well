namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        /*[HttpPost]
        [Route("credit-bulk/{creditEvents}")]
        public HttpResponseMessage BulkCredit(List<CreditEvent> creditEvents)
        {
            try
            {
                var thresholdErrors = this.userThresholdService.RemoveCreditEventsThatDontHaveAThreshold(creditEvents, this.UserIdentityName);

                var response = this.exceptionEventService.BulkCredit(creditEvents, this.UserIdentityName);

                if (response == AdamResponse.AdamDown)
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { adamdown = true, notAcceptable = true, message = thresholdErrors.Select(x => x.Value) });

                if (thresholdErrors.Any())
                    return this.Request.CreateResponse(HttpStatusCode.OK, new { notAcceptable = true, message = thresholdErrors.Select(x => x.Value) });

                return Request.CreateResponse(HttpStatusCode.OK, new { success = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error on bulk credit", exception);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }*/
    }
}