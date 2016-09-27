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
    using Services.Contracts;

    public class DeliveryLineActionsController : BaseApiController
    {
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        private readonly ILogger logger;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IDeliveryService deliveryService;

        public DeliveryLineActionsController(
            IServerErrorResponseHandler serverErrorResponseHandler,
            ILogger logger,
            IJobDetailRepository jobDetailRepository,
            IDeliveryService deliveryService)
        {
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.logger = logger;
            this.jobDetailRepository = jobDetailRepository;
            this.deliveryService = deliveryService;
            this.jobDetailRepository.CurrentUser = UserIdentityName;
        }


        [HttpPost]
        [Route("delivery-line-actions")]
        public HttpResponseMessage Post(DeliveryLineActionsModel model)
        {
            try
            {
                var jobDetail = jobDetailRepository.GetById(model.JobDetailId);

                if (jobDetail == null)
                {
                    logger.LogError($"Unable to update delivery line actions. " +
                                    $"No matching delivery line found for JobDetailId: {model.JobDetailId}.");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ErrorModel()
                    {
                        Message = "Unable to update delivery line actions",
                        Errors = new List<string>()
                        {
                            $"No matching delivery line found for JobDetailId: {model.JobDetailId}."
                        }
                    });
                }

                IEnumerable<JobDetailAction> nonDraftActions = jobDetail.Actions.Where(a => a.Status != ActionStatus.Draft);
                var draftActions = model.DraftActions.Select(a => new JobDetailAction()
                {
                    JobDetailId = model.JobDetailId,
                    Quantity = a.Quantity,
                    Action = a.Action,
                    Status = a.Status
                });
                var actions = nonDraftActions.Concat(draftActions).ToList();
                jobDetail.Actions = new Collection<JobDetailAction>(actions);

                deliveryService.UpdateDraftActions(jobDetail, UserIdentityName);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occured when updating DeliveryLine Actions");
            }
        }
    }
}
