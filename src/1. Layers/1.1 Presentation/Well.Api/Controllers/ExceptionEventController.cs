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

        public ExceptionEventController(ILogger logger, IExceptionEventService exceptionEventService)
        {
            this.logger = logger;
            this.exceptionEventService = exceptionEventService;
        }

        [Route("credit")]
        [HttpPost]
        public HttpResponseMessage Credit(CreditEvent creditEvent)
        {
            try
            {
                var settings = AdamSettingsFactory.GetAdamSettings((Branch)creditEvent.BranchId);

                var response = this.exceptionEventService.Credit(creditEvent, settings, this.UserName);

                if (response == AdamResponse.AdamDown) return this.Request.CreateResponse(HttpStatusCode.OK, new { adamdown = true });

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