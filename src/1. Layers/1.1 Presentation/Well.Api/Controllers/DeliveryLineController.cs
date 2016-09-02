namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Domain;
    using Domain.Enums;
    using Models;
    using Repositories.Contracts;
    using Services.Contracts;

    public class DeliveryLineController : BaseApiController
    {
        private readonly ILogger logger;
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IDeliveryService deliveryService;

        public DeliveryLineController(
            ILogger logger,
            IServerErrorResponseHandler serverErrorResponseHandler,
            IJobDetailRepository jobDetailRepository,
            IDeliveryService deliveryService)
        {
            this.logger = logger;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.jobDetailRepository = jobDetailRepository;
            this.deliveryService = deliveryService;
            this.jobDetailRepository.CurrentUser = UserName;
        }

        [HttpPut]
        [Route("delivery-line")]
        public HttpResponseMessage Update(DeliveryLineModel model)
        {
            try
            {
                JobDetail jobDetail = jobDetailRepository.GetByJobLine(model.JobId, model.LineNo);
                if (jobDetail == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorModel()
                    {
                        Message = "Unable to update delivery line",
                        Errors = new List<string>()
                        {
                            $"No matching delivery line found for JobId: {model.JobId}, LineNumber: {model.LineNo}."
                        }
                    });
                }

                jobDetail.ShortQty = model.ShortQuantity;

                var damages = new Collection<JobDetailDamage>();
                foreach (var damageUpdateModel in model.Damages)
                {
                    var reasonCode = (DamageReasons) Enum.Parse(typeof(DamageReasons), damageUpdateModel.ReasonCode);
                    var damage = new JobDetailDamage
                    {
                        DamageReason = reasonCode,
                        JobDetailId = jobDetail.Id,
                        Qty = damageUpdateModel.Quantity
                    };
                    damages.Add(damage);
                }
                jobDetail.JobDetailDamages = damages;

                deliveryService.UpdateDeliveryLine(jobDetail, UserName);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occcured when updating DeliveryLine", ex);
                return serverErrorResponseHandler.HandleException(Request, ex);
            }
        }

    }
}