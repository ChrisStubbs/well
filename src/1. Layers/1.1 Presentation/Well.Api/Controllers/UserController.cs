namespace PH.Well.Api.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Domain;
    using PH.Well.Services.Contracts;

    public class UserController : BaseApiController
    {
        private readonly IBranchService branchService;

        private readonly IActiveDirectoryService activeDirectoryService;

        public UserController(IBranchService branchService, IActiveDirectoryService activeDirectoryService)
        {
            this.branchService = branchService;
            this.activeDirectoryService = activeDirectoryService;
        }

        [Route("user-branches")]
        [HttpGet]
        public HttpResponseMessage UserBranches()
        {
            var userBranches = this.branchService.GetUserBranchesFriendlyInformation(this.UserName);

            return this.Request.CreateResponse(HttpStatusCode.OK, userBranches);
        }

        [Route("users/{name}")]
        [HttpGet]
        public HttpResponseMessage Users(string name)
        {
            var users = this.activeDirectoryService.FindUsers(name.Trim(), PH.Well.Api.Configuration.DomainsToSearch).ToList();

            if (!users.Any()) users.Add(new User { Id = -1, Name = "No users found!" });

            return this.Request.CreateResponse(HttpStatusCode.OK, users.OrderBy(x => x.Name).ToList());
        }
    }
}
