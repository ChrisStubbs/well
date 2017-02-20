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
        [Route("job-detail-source")]
        public HttpResponseMessage Get()
        {
            try
            {
                var jobDetailSources = Enum<JobDetailSource>.GetValuesAndDescriptions().Select(x => new
                {
                    id = (int)x.Key,
                    description = x.Value
                });

                return Request.CreateResponse(HttpStatusCode.OK, jobDetailSources);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occurred when getting damage reasons");
            }
        }
    }
}
