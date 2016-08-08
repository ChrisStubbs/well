namespace PH.Well.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;

    using PH.Well.Adam.Events;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Services.Contracts;

    public class ExceptionEventController : ApiController
    {
        private readonly ILogger logger;

        private readonly IExceptionEventService exceptionEventService;

        public ExceptionEventController(ILogger logger, IExceptionEventService exceptionEventService)
        {
            this.logger = logger;
            this.exceptionEventService = exceptionEventService;
        }

        [HttpPost]
        public HttpResponseMessage Credit(CreditEvent creditEvent)
        {
            try
            {
                var config = ConfigurationFactory.GetAdamConfiguration((Branch)creditEvent.BranchId);

                var response = this.exceptionEventService.Credit(creditEvent, config);

                return this.Request.CreateResponse(HttpStatusCode.OK, Enum.GetName(typeof(AdamResponse), response));
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error on credit event", exception);
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}