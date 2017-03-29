namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Http;
    using Common.Contracts;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Models;
    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Common.Security;
    using Repositories.Contracts;
    using Services.Contracts;

    public class DeliveryController : BaseApiController
    {
        private readonly IDeliveryReadRepository deliveryReadRepository;
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        private readonly IDeliveryToDetailMapper deliveryToDetailMapper;
        private readonly IDeliveryService deliveryService;

        public DeliveryController(
            IDeliveryReadRepository deliveryReadRepository,
            IServerErrorResponseHandler serverErrorResponseHandler,
            IDeliveryToDetailMapper deliveryToDetailMapper,
            IDeliveryService deliveryService,
            IUserNameProvider userNameProvider) 
            : base(userNameProvider)
        {
            this.deliveryReadRepository = deliveryReadRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.deliveryToDetailMapper = deliveryToDetailMapper;
            this.deliveryService = deliveryService;
        }

        [HttpGet]
        [Route("deliveries/exception")]
        public HttpResponseMessage GetExceptions()
        {
            try
            {
                IList<Delivery> exceptionDeliveries = deliveryService.GetExceptions(this.UserIdentityName);

                return !exceptionDeliveries.Any()
                    ? Request.CreateResponse(HttpStatusCode.NotFound)
                    : Request.CreateResponse(HttpStatusCode.OK, exceptionDeliveries);

            }
            catch (Exception ex)
            {
                return this.serverErrorResponseHandler.HandleException(Request, ex,
                    "An error occured when getting exceptions");
            }
        }

        [HttpGet]
        [Route("deliveries/approval")]
        public HttpResponseMessage GetApprovals()
        {
            try
            {
                var approvals = deliveryService.GetApprovals(this.UserIdentityName);

                return !approvals.Any()
                    ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, approvals);
            }
            catch (Exception ex)
            {
                return this.serverErrorResponseHandler.HandleException(Request, ex,
                    "An error occured when getting approvals");
            }
        }

        [HttpGet]
        [Route("deliveries/clean")]
        public HttpResponseMessage GetCleanDeliveries()
        {
            try
            {
                var cleanDeliveries =
                    this.deliveryReadRepository.GetByStatus(this.UserIdentityName, JobStatus.Clean).ToList();

                return !cleanDeliveries.Any()
                    ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, cleanDeliveries);
            }
            catch (Exception ex)
            {
                return this.serverErrorResponseHandler.HandleException(Request, ex,
                    $"An error occured when getting clean deliveries");
            }
        }

        [HttpGet]
        [Route("deliveries/resolved")]
        public HttpResponseMessage GetResolvedDeliveries()
        {
            try
            {
                List<Delivery> resolvedDeliveries = deliveryReadRepository.GetByStatus(UserIdentityName, JobStatus.Resolved).ToList();

                return !resolvedDeliveries.Any()
                    ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, resolvedDeliveries);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex,
                    "An error occured when getting resolved deliveries");
            }
        }

        [HttpGet]
        [Route("deliveries/{id:int}")]
        public HttpResponseMessage GetDelivery(int id)
        {
            try
            {
                var deliveryDetail = deliveryReadRepository.GetDeliveryById(id, this.UserIdentityName);
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
                return serverErrorResponseHandler.HandleException(Request, ex,
                    $"An error occured when getting delivery detail id: {id}");
            }
        }

        [HttpPost]
        [Route("deliveries/grn")]
        public HttpResponseMessage SaveGrn(GrnModel model)
        {
            this.deliveryService.SaveGrn(model.Id, model.GrnNumber, model.BranchId, UserIdentityName);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}