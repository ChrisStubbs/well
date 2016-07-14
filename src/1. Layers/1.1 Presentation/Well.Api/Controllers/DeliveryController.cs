namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Domain.ValueObjects;
    using Models;
    using PH.Well.Services.Contracts;
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
                //var clean1 = new Delivery
                //{
                //    RouteNumber = "1",
                //    DropId = "C",
                //    InvoiceNumber = "12435",
                //    AccountCode = "5845.621",
                //    AccountName = "Fag Mags and Bags",
                //    JobStatus = "Complete",
                //    DateTime = "20-05-16 14.10"
                //};

                //var clean2 = new Delivery
                //{
                //    RouteNumber = "2",
                //    DropId = "F",
                //    InvoiceNumber = "2363300",
                //    AccountCode = "	7332.000",
                //    AccountName = "Huge Store",
                //    JobStatus = "Complete",
                //    DateTime = "20-05-16 11.25"
                //};

                //var cleanDeliveries = new List<Delivery>();
                //for (int i = 0; i < 100; i++)
                //{
                //    cleanDeliveries.Add(clean1);
                //    cleanDeliveries.Add(clean2);
                //}

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
            //var cleanDeliveries = this.deliveryService.GetResolvedDeliveries().ToList();
            try
            {
                var model1 = new Delivery
                {
                    RouteNumber = "1",
                    DropId = "C",
                    InvoiceNumber = "1365000",
                    AccountCode = "5895.002",
                    AccountName = "Sweet Shop",
                    JobStatus = "Bypassed",
                    Action = "Credited",
                    Assigned = "FLP",
                    DateTime = "20-05-16 11.25"
                };

                var model2 = new Delivery
                {
                    RouteNumber = "1",
                    DropId = "D",
                    InvoiceNumber = "2363292",
                    AccountCode = "5895.002",
                    AccountName = "Fags Mags and Bags",
                    JobStatus = "Short",
                    Action = "Credited",
                    Assigned = "BAD",
                    DateTime = "20-05-16 11.15"
                };

                var model3 = new Delivery
                {
                    RouteNumber = "1",
                    DropId = "E",
                    InvoiceNumber = "22655555",
                    AccountCode = "6898.002",
                    AccountName = "Ye Olde and valued customer",
                    JobStatus = "Damaged",
                    Action = "Replanned",
                    Assigned = "FLP",
                    DateTime = "20-05-16 12.00"
                };

                var resolvedDeliveries = new List<Delivery>();
                for (int i = 0; i < 100; i++)
                {
                    resolvedDeliveries.Add(model1);
                    resolvedDeliveries.Add(model2);
                    resolvedDeliveries.Add(model3);
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, resolvedDeliveries);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting resolved deliveries");
                return serverErrorResponseHandler.HandleException(Request, ex);
            }
        }


    }
}