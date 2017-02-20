namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Common.Contracts;
    using PH.Well.Common.Security;

    using Repositories.Contracts;

    [Authorize(Roles = SecurityPermissions.ActionDeliveries)]
    public class RoutesController : BaseApiController
    {
        private readonly IRouteHeaderRepository routeRepository;
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public RoutesController(IRouteHeaderRepository routeRepository,
                IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.routeRepository = routeRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.routeRepository.CurrentUser = this.UserIdentityName;
        }

        public HttpResponseMessage Get(string searchField = null, string searchTerm = null)
        {
            try
            {
                var routeHeaders = this.routeRepository.GetRouteHeaders();

                if (!routeHeaders.Any())
                {
                    return this.Request.CreateResponse(HttpStatusCode.NotFound);
                }

                var result = routeHeaders
                                    .Select(p => new
                                    {
                                        Route = p.RouteNumber,
                                        RouteDate = p.RouteDate.Value,
                                        TotalDrops = p.TotalDrops,
                                        DeliveryCleanCount = p.CleanJobs,
                                        DeliveryExceptionCount = p.ExceptionJobs,
                                        RouteStatusDescription = p.RouteStatusDescription,
                                        DateTimeUpdated = p.DateUpdated,
                                        RouteOwnerId = p.RouteOwnerId,
                                        DriverName = p.DriverName
                                    })
                                    .ToList();

                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return serverErrorResponseHandler.HandleException(Request, ex, "An error occurred when getting routes");
            }
        }
    }
}
