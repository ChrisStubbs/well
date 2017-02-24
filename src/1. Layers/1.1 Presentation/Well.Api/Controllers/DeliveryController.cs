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
    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Common.Extensions;
    using PH.Well.Common.Security;
    using PH.Well.Domain.Enums;

    using Repositories.Contracts;
    using Services.Contracts;

    [Authorize(Roles = SecurityPermissions.ActionDeliveries)]
    public class DeliveryController : BaseApiController
    {
        private readonly IDeliveryReadRepository deliveryReadRepository;
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        private readonly IDeliveryToDetailMapper deliveryToDetailMapper;
        private readonly ILogger logger;
        private readonly IDeliveryService deliveryService;
        private readonly IJobRepository jobRepository;

        public DeliveryController(
            IDeliveryReadRepository deliveryReadRepository,
            IServerErrorResponseHandler serverErrorResponseHandler,
            IDeliveryToDetailMapper deliveryToDetailMapper,
            ILogger logger,
            IDeliveryService deliveryService,
            IJobRepository jobRepository,
            IUserNameProvider userNameProvider) 
            : base(userNameProvider)
        {
            this.deliveryReadRepository = deliveryReadRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.deliveryToDetailMapper = deliveryToDetailMapper;
            this.logger = logger;
            this.deliveryService = deliveryService;
            this.jobRepository = jobRepository;
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
                    $"An error occured when getting delivery detail id: {id}");
            }
        }

        /*[HttpPost]
        [Route("deliveries/{id:int}/submit-actions")]
        public HttpResponseMessage SubmitActions(int id)
        {
            var job = jobRepository.GetById(id);
            if (job == null)
            {
                logger.LogError($"Unable to submit delivery actions. No matching delivery found for Id: {id}.");
                var errorModel = new ErrorModel
                {
                    Message = "Unable to submit delivery actions",
                    Errors = new List<string>() { $"No matching delivery found for Id: {id}." }
                };
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorModel);
            }

            deliveryService.SubmitActions(id, UserIdentityName);

            return Request.CreateResponse(HttpStatusCode.OK);
        }*/

        [HttpPost]
        [Route("deliveries/grn")]
        public HttpResponseMessage SaveGrn(GrnModel model)
        {
            this.deliveryService.SaveGrn(model.Id, model.GrnNumber, model.BranchId, UserIdentityName);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("delivery-actions")]
        public HttpResponseMessage Get()
        {
            try
            {
                IEnumerable<DeliveryAction> actions = Enum.GetValues(typeof(DeliveryAction)).Cast<DeliveryAction>();
                var reasons = actions
                    .Select(a => new
                    {
                        id = (int)a,
                        description = StringExtensions.GetEnumDescription(a)
                    });

                return Request.CreateResponse(HttpStatusCode.OK, reasons);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occurred when getting delivery actions");
            }
        }

        //[HttpGet]
        //[Route("delivery-podactions")]
        //public HttpResponseMessage GetPodAction()
        //{
        //    try
        //    {
        //        IEnumerable<PodDeliveryAction> actions = Enum.GetValues(typeof(PodDeliveryAction)).Cast<PodDeliveryAction>();
        //        var reasons = actions.Select(a => new { id = (int)a, description = StringExtensions.GetEnumDescription(a) });

        //        return Request.CreateResponse(HttpStatusCode.OK, reasons);
        //    }
        //    catch (Exception ex)
        //    {
        //        return serverErrorResponseHandler.HandleException(Request, ex, "An error occcured when getting pod delivery actions");
        //    }
        //}
    }
}