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
    using Models;
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


        [HttpGet]
        [Route("deliveries/exception", Name = "GetExceptions")]
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

        [HttpGet]
        [Route("deliveries/clean", Name = "GetCleanDeliveries")]
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



        [HttpGet]
        [Route("deliveries/resolved", Name = "GetResolvedDeliveries")]
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

        [HttpGet]
        [Route("deliveries/{id:int}", Name = "GetDelivery")]
        public HttpResponseMessage GetDelivery(int id)
        {
            try
            {
                //Todo: Replace this with call to database and mapping!
                var delivery = this.GetMockDeliveryDetails(id);
                return (delivery == null)
                   ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                   : this.Request.CreateResponse(HttpStatusCode.OK, delivery);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting delivery detail id: {id}");
                return serverErrorResponseHandler.HandleException(Request, ex);
            }
        }


        private DeliveryDetailModel GetMockDeliveryDetails(int id)
        {
            var deliveryDetail = new DeliveryDetailModel
            {
                Id = id,
                AccountCode = "4895.002",
                AccountName = "Fags bags and Mags",
                AccountAddress = "Gaddesdon Place, Great Gaddesdon, Hemel Hempstead HP2 6EX",
                InvoiceNumber = "1363291",
                ContactName = "Chuck Chidlow",
                PhoneNumber = "01234 678901",
                MobileNumber = "07756 914463"
            };

            for (int i = 0; i < 5; i++)
            {
                deliveryDetail.DeliveryLines.Add(new DeliveryLineModel
                {
                    LineNo = i,
                    ProductCode = "19152",
                    ProductDescription = "Blu Cart Ref C/Tob12mg3s",
                    Value = "149.70",
                    InvoicedQuantity = 10,
                    DeliveredQuantity = 7,
                    DamagedQuantity = 0,
                    ShortQuantity = 3,
                    Reason = "Picking Error",
                    Status = "Short",
                    Action = "Credited"
                });
                deliveryDetail.DeliveryLines.Add(new DeliveryLineModel
                {
                    LineNo = i + 1,
                    ProductCode = "19144",
                    ProductDescription = "Blu Cart Ref Gold18mg 3s",
                    Value = "29.94",
                    InvoicedQuantity = 2,
                    DeliveredQuantity = 2,
                    DamagedQuantity = 1,
                    ShortQuantity = 0,
                    Status = "Damaged",
                    Action = "Credited"
                });
                deliveryDetail.DeliveryLines.Add(new DeliveryLineModel
                {
                    LineNo = i + 2,
                    ProductCode = "19136",
                    ProductDescription = "Blu Clearomiser",
                    Value = "299.90",
                    InvoicedQuantity = 19,
                    DeliveredQuantity = 8,
                    DamagedQuantity = 0,
                    ShortQuantity = 11,
                    Reason = "Short deliverd",
                    Status = "Short",
                    Action = "Credited"
                });
                deliveryDetail.DeliveryLines.Add(new DeliveryLineModel
                {
                    LineNo = i + 2,
                    ProductCode = "19500",
                    ProductDescription = "Marlboro Reds 20s",
                    Value = "299.90",
                    InvoicedQuantity = 20,
                    DeliveredQuantity = 20,
                    DamagedQuantity = 0,
                    ShortQuantity = 0,
                    Reason = "Short deliverd",
                    Status = "OK"
                });
            }
            return deliveryDetail;
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