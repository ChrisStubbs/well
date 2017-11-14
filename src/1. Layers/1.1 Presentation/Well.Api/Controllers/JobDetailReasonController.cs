namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Common.Extensions;
    using Domain.Enums;

    public class JobDetailReasonController : BaseApiController
    {
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public JobDetailReasonController(IServerErrorResponseHandler serverErrorResponseHandler, IUserNameProvider userNameProvider)
            : base(userNameProvider)
        {
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [HttpGet]
        [Route("{branchId:int}/damage-reasons")]
        public HttpResponseMessage Get()
        {
            var jobDetailReasons = Enum<JobDetailReason>.GetValuesAndDescriptions().Select(x => new
            {
                id = (int)x.Key,
                description = x.Value
            });

            return Request.CreateResponse(HttpStatusCode.OK, jobDetailReasons);
        }
    }
}