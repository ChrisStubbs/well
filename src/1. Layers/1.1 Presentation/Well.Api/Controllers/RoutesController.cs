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

        public RoutesController(IRouteReadRepository routeReadRepository,
                IUserNameProvider userNameProvider) :
            base(userNameProvider)
        {
            this.routeReadRepository = routeReadRepository;
        }

        public HttpResponseMessage Get(int branchId)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK,
                this.routeReadRepository.GetAllRoutesForBranch(branchId, this.UserIdentityName).ToList());
        }
    }
}
