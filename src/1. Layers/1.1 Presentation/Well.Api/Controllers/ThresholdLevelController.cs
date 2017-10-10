using PH.Well.Services.Contracts;

namespace PH.Well.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.Extensions;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;

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

        [Route("threshold-level")]
        [HttpPost]
        public HttpResponseMessage Post(int thresholdId, string username)
        {
            userThresholdService.SetThresholdLevel(username, thresholdId);
            return this.Request.CreateResponse(HttpStatusCode.Created, new { success = true });
        }
    }
}

