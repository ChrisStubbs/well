namespace PH.Well.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Common.Contracts;
    using Repositories.Contracts;

    public class RouterHeaderController : ApiController
    {

        private readonly ILogger logger;

        private readonly IRouteHeaderRepository routeHeaderRepository;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public RouterHeaderController(ILogger logger, IRouteHeaderRepository routeHeaderRepository,
            IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.logger = logger;
            this.routeHeaderRepository = routeHeaderRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }


        [Route("routeheaders", Name = "GetRouteHeaders")]
        [HttpGet]
        public HttpResponseMessage GetRouteHeaders()
        {
            try
            {
                var routeHeaders = this.routeHeaderRepository.GetRouteHeaders();
                return this.Request.CreateResponse(HttpStatusCode.OK, routeHeaders);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting route headers");
                return serverErrorResponseHandler.HandleException(Request, ex);
            }
        }

    }
}
