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

    public class JobDetailSourceController : ApiController
    {
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public JobDetailSourceController(IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [HttpGet]
        [Route("{branchId:int}/job-detail-source")]
        public HttpResponseMessage Get()
        {
            var jobDetailSources = Enum<JobDetailSource>.GetValuesAndDescriptions().Select(x => new
            {
                id = (int)x.Key,
                description = x.Value
            });

            return Request.CreateResponse(HttpStatusCode.OK, jobDetailSources);

        }
    }
}
