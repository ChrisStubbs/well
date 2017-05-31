namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using Common.Contracts;
    using Repositories.Contracts;

    public class RoutesController : BaseApiController
    {
        private readonly IRouteReadRepository routeReadRepository;
        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public RoutesController(IRouteReadRepository routeReadRepository,
                IServerErrorResponseHandler serverErrorResponseHandler,
                IUserNameProvider userNameProvider) :
            base(userNameProvider)
        {
            this.routeReadRepository = routeReadRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;

        }

        public HttpResponseMessage Get(int branchId)
        {
            try
            {
                return this.Request.CreateResponse(HttpStatusCode.OK,
                    this.routeReadRepository.GetAllRoutesForBranch(branchId, this.UserIdentityName).ToList());
            }
            catch (Exception ex)
            {

                return serverErrorResponseHandler.HandleException(Request, ex, "An error occurred when getting routes");
            }
        }

    }
}
