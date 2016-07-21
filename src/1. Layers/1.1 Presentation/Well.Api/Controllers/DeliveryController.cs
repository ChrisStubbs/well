namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Repositories.Contracts;

    public class DeliveryController : ApiController
    {
        private readonly ILogger logger;
        private readonly IDeliveryReadRepository deliveryReadRepository;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public DeliveryController(
            ILogger logger,
            IDeliveryReadRepository deliveryReadRepository,
            IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.logger = logger;
            this.deliveryReadRepository = deliveryReadRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [Route("deliveries/clean", Name = "GetCleanDeliveries")]
        [HttpGet]
        public HttpResponseMessage GetCleanDeliveries()
        {
            try
            {
                var cleanDeliveries = this.deliveryReadRepository.GetCleanDeliveries().ToList();

                return !cleanDeliveries.Any()
                    ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, cleanDeliveries);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting clean deliveries");
                return this.serverErrorResponseHandler.HandleException(Request, ex);
            }
        }


        [Route("deliveries/resolved", Name = "GetResolvedDeliveries")]
        [HttpGet]
        public HttpResponseMessage GetResolvedDeliveries()
        {
            try
            {
                var resolvedDeliveries = this.deliveryReadRepository.GetResolvedDeliveries().ToList();
                return !resolvedDeliveries.Any()
                   ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                   : this.Request.CreateResponse(HttpStatusCode.OK, resolvedDeliveries);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting resolved deliveries");
                return serverErrorResponseHandler.HandleException(Request, ex);
            }
        }


    }
}