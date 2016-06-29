namespace PH.Well.Api.Controllers
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Common.Contracts;
    using Domain;
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


        [Route("cleanDeliveries", Name = "GetCleanDeliveries")]
        [HttpGet]
        public HttpResponseMessage GetCleanDeliveries()
        {
            try
            {
                var noOfCleanDeliveries = this.routeHeaderRepository.GetCleanDeliveries() ?? new RouteException();
                return this.Request.CreateResponse(HttpStatusCode.OK, noOfCleanDeliveries);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting the number of clean deliveries");
                return serverErrorResponseHandler.HandleException(Request, ex);
            }
        }

        [Route("exceptions", Name = "GetExceptions")]
        [HttpGet]
        public HttpResponseMessage GetExceptions()
        {
            try
            {
                var noOfExceptions = this.routeHeaderRepository.GetExceptions() ?? new RouteException();
                return this.Request.CreateResponse(HttpStatusCode.OK, noOfExceptions);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting the number of exceptions");
                return serverErrorResponseHandler.HandleException(Request, ex);
            }
        }

    }
}
