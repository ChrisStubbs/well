﻿namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Models;
    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Common.Security;

    using Repositories.Contracts;
    using Services.Contracts;

    [Authorize(Roles = SecurityPermissions.ActionDeliveries)]
    public class DeliveryController : BaseApiController
    {
        private readonly IDeliveryReadRepository deliveryReadRepository;
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        private readonly IDeliveryToDetailMapper deliveryToDetailMapper;
        private readonly IJobRepository jobRepository;
        private readonly ILogger logger;
        private readonly IDeliveryService deliveryService;

        public DeliveryController(
            IDeliveryReadRepository deliveryReadRepository,
            IServerErrorResponseHandler serverErrorResponseHandler,
            IDeliveryToDetailMapper deliveryToDetailMapper,
            IJobRepository jobRepository,
            ILogger logger,
            IDeliveryService deliveryService)
        {
            this.deliveryReadRepository = deliveryReadRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.deliveryToDetailMapper = deliveryToDetailMapper;
            this.jobRepository = jobRepository;
            this.logger = logger;
            this.deliveryService = deliveryService;
        }

        [HttpGet]
        [Route("deliveries/exception")]
        public HttpResponseMessage GetExceptions()
        {
            try
            {
                var exceptionDeliveries =
                    this.deliveryReadRepository.GetExceptionDeliveries(this.UserIdentityName).ToList();
                exceptionDeliveries.ForEach(x => x.SetCanAction(this.UserIdentityName));

                return !exceptionDeliveries.Any()
                    ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, exceptionDeliveries);

            }
            catch (Exception ex)
            {
                return this.serverErrorResponseHandler.HandleException(Request, ex,
                    "An error occured when getting exceptions");
            }
        }

        [HttpGet]
        [Route("deliveries/clean")]
        public HttpResponseMessage GetCleanDeliveries()
        {
            try
            {
                List<Delivery> cleanDeliveries =
                    this.deliveryReadRepository.GetCleanDeliveries(this.UserIdentityName).ToList();

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
                var resolvedDeliveries = deliveryReadRepository.GetResolvedDeliveries(UserIdentityName).ToList();

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
                DeliveryDetail deliveryDetail = deliveryReadRepository.GetDeliveryById(id, this.UserIdentityName);
                deliveryDetail.SetCanAction(this.UserIdentityName);

                var deliveryLines = deliveryReadRepository.GetDeliveryLinesByJobId(id);
                DeliveryDetailModel delivery = this.deliveryToDetailMapper.Map(deliveryLines, deliveryDetail);

                if (delivery.AccountCode.Length <= 0)
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, delivery);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex,
            }
        }

        [HttpPost]
        [Route("deliveries-creditlines/{creditlines}")]
        public HttpResponseMessage CreditLines(IEnumerable<int> creditLines)
        {
            try
            { 
                if (creditLines == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorModel()
                    {
                        Message = "No I'ds given for updated credited job lines",
                        Errors = new List<string>()
                        {
                            $"No Id's given for updated credited job lines."
                        }
                    });
                }

                this.jobRepository.CreditLines(creditLines);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occured when crediting Job lines");
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occured when crediting Job lines");
            }

            deliveryService.SubmitActions(id, UserIdentityName);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}