namespace PH.Well.Api.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Services.Contracts;

    public class UserController : BaseApiController
    {
        private readonly IBranchService branchService;

        public UserController(IBranchService branchService)
        {
            this.branchService = branchService;
        }

        [Route("user-branches")]
        [HttpGet]
        public HttpResponseMessage UserBranches()
        {
            var userBranches = this.branchService.GetUserBranchesFriendlyInformation(this.UserName);

            return this.Request.CreateResponse(HttpStatusCode.OK, userBranches);
        }
    }
}
