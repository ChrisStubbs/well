namespace PH.Well.Api.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;
    using PH.Well.Services.Contracts;

    public class ActionDeliveryLinesController : BaseApiController
    {
        private readonly IDeliveryReadRepository deliveryRepository;

        private readonly IDeliveryLinesToModelMapper mapper;

        private readonly IDeliveryLineActionService deliveryLineActionService;

        private readonly IJobRepository jobRepository;

        private readonly IBranchRepository branchRepository;

        public ActionDeliveryLinesController(
            IDeliveryReadRepository deliveryRepository,
            IDeliveryLinesToModelMapper mapper,
            IDeliveryLineActionService deliveryLineActionService,
            IJobRepository jobRepository,
            IBranchRepository branchRepository)
        {
            this.deliveryRepository = deliveryRepository;
            this.mapper = mapper;
            this.deliveryLineActionService = deliveryLineActionService;
            this.jobRepository = jobRepository;
            this.branchRepository = branchRepository;
        }

        [Route("delivery-line-actions/{jobId:int}")]
        [HttpGet]
        public HttpResponseMessage DeliveryLineActions(int jobId)
        {
            var deliveryLines = this.deliveryRepository.GetDeliveryLinesByJobId(jobId);

            var model = this.mapper.Map(deliveryLines);

            return this.Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [Route("confirm-delivery-lines/{jobId:int}")]
        [HttpPost]
        public HttpResponseMessage ConfirmDeliveryLines(int jobId)
        {
            var deliveryLines = this.deliveryRepository.GetDeliveryLinesByJobId(jobId);

            if (!deliveryLines.Any())
                return this.Request.CreateResponse(
                    HttpStatusCode.OK,
                    new { notAcceptable = true, message = $"No delivery lines found for job id ({jobId})..." });

            var job = this.jobRepository.GetById(jobId);

            if (job == null)
                return this.Request.CreateResponse(
                    HttpStatusCode.OK,
                    new { notAcceptable = true, message = $"No job found for Id ({jobId})..." });

            var branchId = this.branchRepository.GetBranchIdForJob(jobId);

            var settings = AdamSettingsFactory.GetAdamSettings((Branch)branchId);

            var response = this.deliveryLineActionService.ProcessDeliveryActions(
                deliveryLines.ToList(),
                settings,
                this.UserIdentityName,
                branchId);

            if (response.CreditThresholdLimitReached)
                return this.Request.CreateResponse(
                    HttpStatusCode.OK,
                    new
                    {
                        notAcceptable = true,
                        message =
                        "Your threshold level isn\'t high enough for the credit... It has been passed on for authorisation..."
                    });

            if (response.ThresholdError)
                return this.Request.CreateResponse(
                    HttpStatusCode.OK,
                    new { notAcceptable = true, message = response.ThresholdErrorMessage });

            if (response.AdamResponse == AdamResponse.AdamDown)
                return this.Request.CreateResponse(HttpStatusCode.OK, new { adamdown = true });

            return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
        }
    }
}