namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;

    using PH.Well.Api.Mapper.Contracts;

    using Repositories.Contracts;

    public class RouteController : BaseApiController
    {
        private readonly IRouteHeaderRepository routeRepository;
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;
        private readonly IRouteModelsMapper routeModelsMapper;

        public RouteController(IRouteHeaderRepository routeRepository,
                IServerErrorResponseHandler serverErrorResponseHandler, 
                IRouteModelsMapper routeModelsMapper)
        {
            this.routeRepository = routeRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.routeModelsMapper = routeModelsMapper;
            this.routeRepository.CurrentUser = this.UserName;
        }

        [Route("routes", Name = "GetRoutes")]
        [HttpGet]
        public HttpResponseMessage GetRoutes(string searchField = null, string searchTerm = null)
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
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occcured when getting routes");
            }
        }
    }
}
