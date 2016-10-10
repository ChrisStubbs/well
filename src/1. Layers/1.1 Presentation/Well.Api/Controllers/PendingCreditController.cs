namespace PH.Well.Api.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Repositories.Contracts;

    public class PendingCreditController : BaseApiController
    {
        private readonly IDeliveryReadRepository deliveryReadRepository;

        public PendingCreditController(IDeliveryReadRepository deliveryReadRepository)
        {
            this.deliveryReadRepository = deliveryReadRepository;
        }

        [Route("pending-credits")]
        [HttpGet]
        public HttpResponseMessage Get()
        {
            var pendingCredits = this.deliveryReadRepository.GetPendingCreditDeliveries(this.UserIdentityName);

            return this.Request.CreateResponse(HttpStatusCode.OK, pendingCredits);
        }

        [Route("pending-credit-detail/{jobId:int}")]
        [HttpGet]
        public HttpResponseMessage GetDetails(int jobId)
        {
            var pendingCreditDetails = this.deliveryReadRepository.GetPendingCreditDetail(jobId);

            return this.Request.CreateResponse(HttpStatusCode.OK, pendingCreditDetails);
        }
    }
}