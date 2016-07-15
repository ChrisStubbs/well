namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using Mapper;
    using Repositories.Contracts;

    public class RouteController : ApiController
    {
        private readonly ILogger logger;

        private readonly IRouteHeaderRepository routeRepository;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        private readonly IRouteModelsMapper routeModelsMapper;

        public RouteController(ILogger logger, IRouteHeaderRepository routeRepository,
            IServerErrorResponseHandler serverErrorResponseHandler, IRouteModelsMapper routeModelsMapper)
        {
            this.logger = logger;
            this.routeRepository = routeRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.routeModelsMapper = routeModelsMapper;
        }

        [Route("routes", Name = "GetRoutes")]
        [HttpGet]
        public HttpResponseMessage GetRoutes()
        {
            try
            {
                var routeHeaders = this.routeRepository.GetRouteHeaders().ToArray();
                if (!routeHeaders.Any()) return this.Request.CreateResponse(HttpStatusCode.NotFound);
                var routes = routeModelsMapper.Map(routeHeaders);

                return this.Request.CreateResponse(HttpStatusCode.OK, routes);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"An error occcured when getting routes");
                return serverErrorResponseHandler.HandleException(Request, ex);
            }
        }
    }
}
