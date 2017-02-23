namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using Common.Contracts;
    using Models;
    using Mapper.Contracts;
    using Repositories.Contracts;
    using Services.Contracts;

    public class DeliveryLineController : BaseApiController
    {
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IDeliveryService deliveryService;
        private readonly IDeliveryLineToJobDetailMapper deliveryLineToJobDetailMapper;

        public DeliveryLineController(
            IServerErrorResponseHandler serverErrorResponseHandler,
            IJobDetailRepository jobDetailRepository,
            IDeliveryService deliveryService,
            IDeliveryLineToJobDetailMapper deliveryLineToJobDetailMapper,
            IUserNameProvider userNameProvider)
            : base(userNameProvider)
        {
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.jobDetailRepository = jobDetailRepository;
            this.deliveryService = deliveryService;
            this.deliveryLineToJobDetailMapper = deliveryLineToJobDetailMapper;
            //////this.jobDetailRepository.CurrentUser = UserIdentityName;
        }

        public HttpResponseMessage Put(DeliveryLineModel model)
        {
            try
            {
                var jobDetail = jobDetailRepository.GetByJobLine(model.JobId, model.LineNo);

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

                this.deliveryLineToJobDetailMapper.Map(model, jobDetail);

                deliveryService.UpdateDeliveryLine(jobDetail, UserNameProvider.GetUserName());

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occurred when updating DeliveryLine");
            }
        }

    }
}