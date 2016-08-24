namespace PH.Well.Api.Controllers
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
    using Models;
    using Repositories.Contracts;
    using WebGrease.Css.Extensions;

    public class DeliveryLineController : BaseApiController
    {
        private readonly ILogger logger;
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        private readonly IJobDetailRepository jobDetailRepository;

        public DeliveryLineController(
            ILogger logger,
            IServerErrorResponseHandler serverErrorResponseHandler,
            IJobDetailRepository jobDetailRepository)
        {
            this.logger = logger;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.jobDetailRepository = jobDetailRepository;
        }

        [HttpPut]
        [Route("delivery-line")]
        public HttpResponseMessage Update(DeliveryLineUpdateModel model)
        {
            try
            {
                var jobDetail = jobDetailRepository.GetByJobLine(model.JobId, model.LineNumber);

                if (jobDetail == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorModel()
                    {
                        Message = "Unable to update delivery line",
                        Errors = new List<string>()
                        {
                            $"No matching delivery line found for JobId: {model.JobId}, LineNumber: {model.LineNumber}."
                        }
                    });
                }

                jobDetail.ShortQty = model.ShortQuantity;

                var damages = new Collection<JobDetailDamage>();
                foreach (var damageUpdateModel in model.Damages)
                {
                    var reasonCode = (DamageReasons) Enum.Parse(typeof(DamageReasons), damageUpdateModel.ReasonCode);
                    var damage = jobDetail.JobDetailDamages.SingleOrDefault(d => d.Reason == reasonCode) ??
                                 new JobDetailDamage() {Reason = reasonCode};
                    damage.Qty = damageUpdateModel.Quantity;
                    damages.Add(damage);
                }
                jobDetail.JobDetailDamages = damages;

                jobDetailRepository.JobDetailCreateOrUpdate(jobDetail);
                foreach (var jobDetailDamage in jobDetail.JobDetailDamages)
                {
                    jobDetailRepository.CreateOrUpdateJobDetailDamage(jobDetailDamage);
                }

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