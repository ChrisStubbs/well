namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;

    using PH.Well.Api.Models;

    using Repositories.Contracts;

    public class RouteController : ApiController
    {
        private readonly ILogger logger;

        private readonly IRouteHeaderRepository routeRepository;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public RouteController(ILogger logger, IRouteHeaderRepository routeRepository,
            IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.logger = logger;
            this.routeRepository = routeRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
        }

        [Route("routes", Name = "GetRoutes")]
        [HttpGet]
        public HttpResponseMessage GetRoutes()
        {
            try
            {
                /*var routes = this.routeRepository.GetRouteHeaders();

                if (!routes.Any()) return this.Request.CreateResponse(HttpStatusCode.NotFound);*/

                var model = new RouteModel
                {
                    Route = "1",
                    DriverName = "Leeroy Brown",
                    DeliveryExceptionCount = 43,
                    DeliveryCleanCount = 2,
                    RouteStatus = "In Progress",
                    TotalDrops = 1234,
                    DateTimeUpdated = "12 january 2016"
                };

                var model2 = new RouteModel
                {
                    Route = "2",
                    DriverName = "Shirley Vallentine",
                    DeliveryExceptionCount = 10,
                    DeliveryCleanCount = 200,
                    RouteStatus = "Complete",
                    TotalDrops = 50,
                    DateTimeUpdated = "12 february 2016"
                };

                var model3 = new RouteModel
                {
                    Route = "3",
                    DriverName = "Miley Cirus",
                    DeliveryExceptionCount = 12,
                    DeliveryCleanCount = 100,
                    RouteStatus = "Complete",
                    TotalDrops = 20,
                    DateTimeUpdated = "12 february 2016"
                };

                var routes = new List<RouteModel> { model, model2, model3, model, model2, model3, model, model2, model3 };

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
