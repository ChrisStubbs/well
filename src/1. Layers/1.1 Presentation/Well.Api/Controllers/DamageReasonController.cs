namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Common.Extensions;
    using Domain.Enums;

    public class DamageReasonController : BaseApiController
    {
        private readonly ILogger logger;
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public DamageReasonController(
            ILogger logger,
            IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.logger = logger;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [HttpGet]
        [Route("damage-reasons")]
        public HttpResponseMessage Get()
        {
            try
            {
                var reasons = Enum.GetNames(typeof(DamageReasons)).Select(r => new
                {
                    code = r,
                    description = StringExtensions.GetEnumDescription<DamageReasons>(r)
                });
                return Request.CreateResponse(HttpStatusCode.OK, reasons);
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occcured when getting damage reasons", ex);
                return serverErrorResponseHandler.HandleException(Request, ex);
            }
        }

    }
}