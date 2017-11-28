using PH.Well.Services.Contracts;

namespace PH.Well.Api.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Common.Contracts;

    public class ThresholdLevelController : BaseApiController
    {
        private readonly ILogger logger;
        private readonly IUserThresholdService userThresholdService;

        public ThresholdLevelController(ILogger logger, IUserNameProvider userNameProvider, IUserThresholdService userThresholdService) :
            base(userNameProvider)
        {
            this.logger = logger;
            this.userThresholdService = userThresholdService;
        }

        [Route("{branchId:int}/threshold-level")]
        [Route("threshold-level")]
        [HttpPost]
        public HttpResponseMessage Post(int thresholdId, string username)
        {
            userThresholdService.SetThresholdLevelAllDatabases(username, thresholdId);
            return this.Request.CreateResponse(HttpStatusCode.Created, new { success = true });
        }
    }
}

