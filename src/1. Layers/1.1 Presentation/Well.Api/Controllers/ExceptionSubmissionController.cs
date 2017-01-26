namespace PH.Well.Api.Controllers
{
    using System;
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

    public class ExceptionSubmissionController : BaseApiController
    {
        private readonly ILogger logger;

        private readonly IDeliveryReadRepository deliveryRepository;

        private readonly IDeliveryLinesToModelMapper mapper;

        private readonly IExceptionEventService exceptionEventService;

        private readonly IJobRepository jobRepository;

        private readonly IBranchRepository branchRepository;

        public ExceptionSubmissionController(
            ILogger logger,
            IDeliveryReadRepository deliveryRepository,
            IDeliveryLinesToModelMapper mapper,
            IExceptionEventService exceptionEventService,
            IJobRepository jobRepository,
            IBranchRepository branchRepository)
        {
            this.logger = logger;
            this.deliveryRepository = deliveryRepository;
            this.mapper = mapper;
            this.exceptionEventService = exceptionEventService;
            this.jobRepository = jobRepository;
            this.branchRepository = branchRepository;
        }

        [Route("submission-confirm/{jobId:int}")]
        [HttpGet]
        public HttpResponseMessage GetConfirmationDetails(int jobId)
        {
            var deliveryLines = this.deliveryRepository.GetDeliveryLinesByJobId(jobId);

            var model = this.mapper.Map(deliveryLines);

            return this.Request.CreateResponse(HttpStatusCode.OK, model);
        }

        [Route("confirm-exceptions/{jobId:int}")]
        [HttpPost]
        public HttpResponseMessage Confirm(int jobId)
        {
            try
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
                        new { notAcceptable = true, message = $"No job found for Id ({jobId})" });

                var branchId = this.branchRepository.GetBranchIdForJob(jobId);

                var settings = AdamSettingsFactory.GetAdamSettings((Branch)branchId);

                var response = this.exceptionEventService.ProcessDeliveryActions(
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
                            "Your threshold level isn\'t high enough to credit this! It has been passed on for authorisation!"
                        });

                if (response.AdamResponse == AdamResponse.AdamDown) return this.Request.CreateResponse(HttpStatusCode.OK, new { adamdown = true });

                if (response.AdamResponse == AdamResponse.PartProcessed) return this.Request.CreateResponse(HttpStatusCode.OK, new { adamPartProcessed = true });

                return this.Request.CreateResponse(HttpStatusCode.OK, new { success = true });
            }
            catch (UserThresholdNotFoundException)
            {
                return this.Request.CreateResponse(
                    HttpStatusCode.OK,
                    new
                    {
                        notAcceptable = true,
                        message = "User for next level of threshold to assign to was not found..."
                    });
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when processing delivery line actions...", exception);
                return this.Request.CreateResponse(
                    HttpStatusCode.OK,
                    new { notAcceptable = true, message = "Error when processing delivery line actions..." });
            }
        }
    }
}