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

        [Route("deliveries/exception", Name = "GetExceptions")]
        [HttpGet]
        public HttpResponseMessage GetExceptions()
        {
            try
            {
                // var exceptionDeliveries = this.deliveryReadRepository.GetExceptionDeliveries().ToList();
                var exception1 = new Delivery
                {
                    RouteNumber = "1",
                    DropId = "C",
                    InvoiceNumber = "12435",
                    AccountCode = "5845.621",
                    AccountName = "Fag Mags and Bags",
                    JobStatus = "Incomplete",
                    DateTime = "20-05-16 14.10",
                    AccountId = "2"
                };

                var exception2 = new Delivery
                {
                    RouteNumber = "2",
                    DropId = "F",
                    InvoiceNumber = "2363300",
                    AccountCode = "	7332.000",
                    AccountName = "Huge Store",
                    JobStatus = "Incomplete",
                    DateTime = "20-05-16 11.25",
                    AccountId = "2"
                };

                var exceptionDeliveries = new List<Delivery>();
                for (int i = 0; i < 100; i++)
                {
                    exceptionDeliveries.Add(exception1);
                    exceptionDeliveries.Add(exception2);
                }

                return !exceptionDeliveries.Any()
                    ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, exceptionDeliveries);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting clean deliveries");
                return this.serverErrorResponseHandler.HandleException(Request, ex);
            }
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