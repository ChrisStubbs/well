namespace PH.Well.Api.Controllers
{
    using System;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Models;
    using Repositories.Contracts;
    using Services.Contracts;

    public class DeliveryLineActionsController : BaseApiController
    {
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        private readonly IJobDetailRepository jobDetailRepository;
        private readonly IDeliveryService deliveryService;

        public DeliveryLineActionsController(
            IServerErrorResponseHandler serverErrorResponseHandler,
            IJobDetailRepository jobDetailRepository,
            IDeliveryService deliveryService)
        {
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.jobDetailRepository = jobDetailRepository;
            this.deliveryService = deliveryService;
            this.jobDetailRepository.CurrentUser = UserIdentityName;
        }


        [HttpPost]
        [Route("delivery-line-actions")]
        public HttpResponseMessage Post(DeliveryLineActionsModel model)
        {
            throw new NotImplementedException();

        }
    }
}
