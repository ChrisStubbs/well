namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Models;
    using Repositories.Contracts;

    public class DeliveryController : BaseApiController
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
               var exceptionDeliveries = this.deliveryReadRepository.GetExceptionDeliveries(this.UserName).ToList();

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
                var cleanDeliveries = this.deliveryReadRepository.GetCleanDeliveries(this.UserName).ToList();

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
                var resolvedDeliveries = this.deliveryReadRepository.GetResolvedDeliveries(this.UserName).ToList();
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
                return this.Request.CreateResponse(HttpStatusCode.OK);
             //   var delivery = this.GetMockDeliveryDetails(id);

                var deliveryDetail = this.deliveryReadRepository.GetDeliveryById(id);
                var deliveryLines = this.deliveryReadRepository.GetDeliveryLinesById(id);
                var delivery = CreateDeliveryDetails(deliveryLines, deliveryDetail);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting delivery detail id: {id}");
                return serverErrorResponseHandler.HandleException(Request, ex);
            }
        }


        private DeliveryDetailModel CreateDeliveryDetails(IEnumerable<DeliveryLine> lines, DeliveryDetail detail )
        {
            var deliveryDetail = new DeliveryDetailModel
            {
                Id = detail.Id,
                AccountCode = detail.AccountCode,
                AccountName = detail.AccountName,
                AccountAddress = detail.AccountAddress,
                InvoiceNumber = detail.InvoiceNumber,
                ContactName = detail.ContactName,
                PhoneNumber = detail.PhoneNumber,
                MobileNumber = detail.MobileNumber,
                DeliveryType = detail.DeliveryType
            };

            foreach (var line in lines)
            {
                deliveryDetail.DeliveryLines.Add(new DeliveryLineModel
                {
                    LineNo = line.LineNo,
                    ProductCode = line.ProductCode,
                    ProductDescription = line.ProductDescription,
                    Value = line.Value.ToString(),
                    InvoicedQuantity = line.InvoicedQuantity,
                    DeliveredQuantity = line.DeliveredQuantity,
                    DamagedQuantity = line.DamagedQuantity,
                    ShortQuantity = line.ShortQuantity,
                    Reason = line.Reason,
                    Status = line.Status
                });
            }

            return deliveryDetail;
        }


    }
}