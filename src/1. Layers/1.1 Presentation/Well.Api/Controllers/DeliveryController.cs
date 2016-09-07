namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using PH.Well.Api.Mapper.Contracts;
    using Repositories.Contracts;

    public class DeliveryController : BaseApiController
    {
        private readonly IDeliveryReadRepository deliveryReadRepository;
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        private readonly IDeliveryToDetailMapper deliveryToDetailMapper;

        public DeliveryController(
            IDeliveryReadRepository deliveryReadRepository,
            IServerErrorResponseHandler serverErrorResponseHandler,
            IDeliveryToDetailMapper deliveryToDetailMapper)
        {
            this.deliveryReadRepository = deliveryReadRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.deliveryToDetailMapper = deliveryToDetailMapper;
        }

        [HttpGet]
        [Route("deliveries/exception")]
        public HttpResponseMessage GetExceptions()
        {
            try
            {
                var exceptionDeliveries = this.deliveryReadRepository.GetExceptionDeliveries(this.UserIdentityName).ToList();
                exceptionDeliveries.ForEach(x => x.SetCanAction(this.UserIdentityName));

                return !exceptionDeliveries.Any()
                    ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, exceptionDeliveries);

            }
            catch (Exception ex)
            {
                return this.serverErrorResponseHandler.HandleException(Request, ex, "An error occured when getting exceptions");
            }
        }

       

        [HttpGet]
        [Route("deliveries/clean")]
        public HttpResponseMessage GetCleanDeliveries()
        {
            try
            {
                var cleanDeliveries = this.deliveryReadRepository.GetCleanDeliveries(this.UserIdentityName).ToList();

                return !cleanDeliveries.Any()
                    ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, cleanDeliveries);
            }
            catch (Exception ex)
            {
                return this.serverErrorResponseHandler.HandleException(Request, ex, $"An error occured when getting clean deliveries for route {routeId}");
            }
        }

        [HttpGet]
        [Route("deliveries/resolved")]
        public HttpResponseMessage GetResolvedDeliveries()
        {
            try
            {
                var resolvedDeliveries = deliveryReadRepository.GetResolvedDeliveries(UserIdentityName).ToList();

                return !resolvedDeliveries.Any()
                   ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                   : this.Request.CreateResponse(HttpStatusCode.OK, resolvedDeliveries);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occured when getting resolved deliveries");
            }
        }

        [HttpGet]
        [Route("deliveries/{id:int}")]
        public HttpResponseMessage GetDelivery(int id)
        {
            try
            {
                var deliveryDetail = deliveryReadRepository.GetDeliveryById(id, this.UserIdentityName);
                deliveryDetail.SetCanAction(this.UserIdentityName);

                var deliveryLines = deliveryReadRepository.GetDeliveryLinesByJobId(id);
                var delivery = this.deliveryToDetailMapper.Map(deliveryLines, deliveryDetail);

                if (delivery.AccountCode.Length <= 0)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, delivery);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, $"An error occured when getting delivery detail id: {id}");
            }
        }
    }
}