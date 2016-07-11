namespace PH.Well.Api.Controllers
{
    using Common.Contracts;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class ResolvedDeliveryController : ApiController
    {
        private readonly ILogger logger;
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public ResolvedDeliveryController(ILogger logger,IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.logger = logger;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [Route("deliveries/resolved", Name = "GetResolvedDeliveries")]
        [HttpGet]
        public HttpResponseMessage GetResolvedDeliveries()
        {
            try
            {
                var model1 = new ResolvedDeliveryModel
                {
                    Route = "1",
                    Drop = "C",
                    InvoiceNo = 1365000,
                    Account = "5895.002",
                    AccountName = "Sweet Shop",
                    Status = "Bypassed",
                    Action = "Credited",
                    Assigned = "FLP",
                    DateTime = "20-05-16 11.25"
                };

                var model2 = new ResolvedDeliveryModel
                {
                    Route = "1",
                    Drop = "D",
                    InvoiceNo = 2363292,
                    Account = "5895.002",
                    AccountName = "Fags Mags and Bags",
                    Status = "Short",
                    Action = "Credited",
                    Assigned = "BAD",
                    DateTime = "20-05-16 11.15"
                };

                var model3 = new ResolvedDeliveryModel
                {
                    Route = "1",
                    Drop = "E",
                    InvoiceNo = 22655555,
                    Account = "6898.002",
                    AccountName = "Ye Olde and valued customer",
                    Status = "Damaged",
                    Action = "Replanned",
                    Assigned = "FLP",
                    DateTime = "20-05-16 12.00"
                };

                var resolvedDeliveries = new List<ResolvedDeliveryModel>();
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
