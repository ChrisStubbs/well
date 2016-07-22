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
    using Repositories.Contracts;
    using Well.Domain.ValueObjects;

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
                var exceptionDeliveries = GetMockDeliveries();
                // var exceptionDeliveries = this.deliveryReadRepository.GetExceptionDeliveries().ToList();

                return !exceptionDeliveries.Any()
                    ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, exceptionDeliveries);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting exceptions");
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


        private List<Delivery> GetMockDeliveries()
        {
            var list = new List<Delivery>();
            var model1 = new Delivery
            {
                RouteNumber = "001",
                DropId = "01",
                InvoiceNumber = "1363291",
                AccountCode = "4895.002",
                AccountName = "Fags bags and Mags",
                JobStatus = "Incomplete",
                Reason = "ByPassed",
                Assigned = "FLP",
                DateTime = DateTime.Now.ToDashboardDateFormat()
            };

            var model2 = new Delivery
            {
                RouteNumber = "001",
                DropId = "01",
                InvoiceNumber = "1363292",
                AccountCode = "4895.002",
                AccountName = "Fags bags and Mags",
                JobStatus = "Pending",
                Reason = "Short",
                Assigned = "FLP",
                DateTime = DateTime.Now.ToDashboardDateFormat()
            };

            var model3 = new Delivery
            {
                RouteNumber = "001",
                DropId = "01",
                InvoiceNumber = "2263287",
                AccountCode = "4895.002",
                AccountName = "Fags bags and Mags",
                JobStatus = "Incomplete",
                Reason = "Short",
                Assigned = "N/A",
                DateTime = DateTime.Now.ToDashboardDateFormat()
            };


            for (int i = 0; i < 67; i++)
            {
                list.Add(model1);
                list.Add(model2);
                list.Add(model3);
            }

            return list;
        }

    }
}