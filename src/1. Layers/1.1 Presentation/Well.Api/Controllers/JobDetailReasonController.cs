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

        public JobDetailReasonController(IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [HttpGet]
        [Route("damage-reasons")]
        public HttpResponseMessage Get()
        {
            try
            {
                var jobDetailReasons = Enum<JobDetailReason>.GetValuesAndDescriptions().Select(x => new
                {
                    id = (int)x.Key,
                    description = x.Value
                });

                return Request.CreateResponse(HttpStatusCode.OK, jobDetailReasons);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occcured when getting damage reasons");
            }
        }

        //[HttpGet]
        //[Route("pod-reasons")]
        //public HttpResponseMessage GetPodReasons()
        //{
        //    try
        //    {
        //        var podReasons = Enum<PodReason>.GetValuesAndDescriptions().Select(x => new
        //        {
        //            id = (int)x.Key,
        //            description = x.Value
        //        });

        //        return Request.CreateResponse(HttpStatusCode.OK, podReasons);
        //    }
        //    catch (Exception ex)
        //    {
        //        return serverErrorResponseHandler.HandleException(Request, ex, "An error occcured when getting pod reasons");
        //    }
        //}

    }
}