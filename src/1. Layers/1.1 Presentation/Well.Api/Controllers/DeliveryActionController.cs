namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Common.Extensions;
    using Domain.Enums;

    public class DeliveryActionController : ApiController
    {
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public DeliveryActionController(IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [HttpGet]
        [Route("delivery-actions")]
        public HttpResponseMessage Get()
        {
            try
            {
                IEnumerable<DeliveryAction> actions = new List<DeliveryAction>()
                {
                    DeliveryAction.NotDefined,
                    DeliveryAction.Credit,
                    DeliveryAction.MarkAsBypassed,
                    DeliveryAction.MarkAsDelivered
                };
                var reasons = actions
                    .Select(a => new
                    {
                        id = (int)a,
                        description = StringExtensions.GetEnumDescription(a)
                    });

                return Request.CreateResponse(HttpStatusCode.OK, reasons);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occurred when getting delivery actions");
            }
        }
    }
}
