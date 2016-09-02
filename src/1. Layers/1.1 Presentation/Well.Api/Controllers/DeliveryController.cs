namespace PH.Well.Api.Controllers
{
    using System.Collections.Generic;
    using PH.Well.Domain.ValueObjects;
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
               List<Delivery> exceptionDeliveries = this.deliveryReadRepository.GetExceptionDeliveries(this.UserName).ToList();

                return !exceptionDeliveries.Any()
                    ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, exceptionDeliveries);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting exceptions", ex);
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
                this.logger.LogError($"An error occcured when getting clean deliveries", ex);
                return this.serverErrorResponseHandler.HandleException(Request, ex);
            }
        }

        [HttpGet]
        [Route("deliveries/resolved", Name = "GetResolvedDeliveries")]
        public HttpResponseMessage GetResolvedDeliveries()
        {
            try
            {
                IEnumerable<Delivery> resolvedDeliveries = deliveryReadRepository.GetResolvedDeliveries(UserName).ToList();
                return !resolvedDeliveries.Any()
                   ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                   : this.Request.CreateResponse(HttpStatusCode.OK, resolvedDeliveries);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting resolved deliveries", ex);
                return serverErrorResponseHandler.HandleException(Request, ex);
            }
        }

        [HttpGet]
        [Route("deliveries/{id:int}", Name = "GetDelivery")]
        public HttpResponseMessage GetDelivery(int id)
        {
            try
            {
                DeliveryDetail deliveryDetail = deliveryReadRepository.GetDeliveryById(id);
                IEnumerable<DeliveryLine> deliveryLines = deliveryReadRepository.GetDeliveryLinesByJobId(id);
                DeliveryDetailModel delivery = CreateDeliveryDetails(deliveryLines, deliveryDetail);

                if (delivery.AccountCode.Length <= 0)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }
                else
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK, delivery);
                }

            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting delivery detail id: {id}", ex);
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
                DeliveryType = detail.DeliveryType,
                IsException = detail.IsException
            };

            foreach (var line in lines)
            {
                deliveryDetail.DeliveryLines.Add(new DeliveryLineModel
                {
                    JobId = line.JobId,
                    LineNo = line.LineNo,
                    ProductCode = line.ProductCode,
                    ProductDescription = line.ProductDescription,
                    Value = line.Value.ToString(),
                    InvoicedQuantity = line.InvoicedQuantity,
                    DeliveredQuantity = line.DeliveredQuantity,
                    DamagedQuantity = line.DamagedQuantity,
                    ShortQuantity = line.ShortQuantity,
                    Damages = line.Damages.Select(d => new DamageModel()
                    {
                        Quantity = d.Quantity,
                        ReasonCode = d.Reason.ToString()
                    }).ToList()
                });
            }

            return deliveryDetail;
        }


    }
}