namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Common.Contracts;

    using PH.Well.Services.Contracts;

    using Repositories.Contracts;


    public class CleanDeliveryController : ApiController
    {
        private readonly ILogger logger;

        private readonly IDeliveryService deliveryService;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public CleanDeliveryController(
            ILogger logger,
            IDeliveryService deliveryService,
            IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.logger = logger;
            this.deliveryService= deliveryService;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [Route("clean", Name = "GetCleanDeliveries")]
        [HttpGet]
        public HttpResponseMessage GetCleanDeliveries()
        {
            try
            {
               
                 var cleanDeliveries = this.deliveryService.GetCleanDeliveries().ToList();

                if (!cleanDeliveries.Any()) return this.Request.CreateResponse(HttpStatusCode.NotFound);
                else
                return this.Request.CreateResponse(HttpStatusCode.OK, cleanDeliveries);

            }
            catch (Exception ex)
            {
                this.logger.LogError ($"An error occcured when getting clean deliveries");
                return this.serverErrorResponseHandler.HandleException(Request, ex);
            }
        }


    }
}