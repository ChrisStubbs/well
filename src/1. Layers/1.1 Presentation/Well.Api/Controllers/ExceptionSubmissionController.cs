namespace PH.Well.Api.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Api.Mapper.Contracts;
    using PH.Well.Common.Contracts;
    using PH.Well.Repositories.Contracts;

    public class ExceptionSubmissionController : BaseApiController
    {
        private readonly ILogger logger;
        private readonly IDeliveryReadRepository deliveryRepository;
        private readonly IDeliveryLinesToModelMapper mapper;

        public ExceptionSubmissionController(
            ILogger logger, 
            IDeliveryReadRepository deliveryRepository,
            IDeliveryLinesToModelMapper mapper)
        {
            this.logger = logger;
            this.deliveryRepository = deliveryRepository;
            this.mapper = mapper;
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
            var deliveryLines = this.deliveryRepository.GetDeliveryLinesByJobId(jobId);

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}