using System;
using System.Threading;
using PH.Well.Domain;
using PH.Well.Repositories.Contracts;

namespace PH.Well.Api.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Services.Contracts;

    public class UserController : BaseApiController
    {
        private readonly IBranchService branchService;
        private readonly IUserRepository userRepository;

        public UserController(IBranchService branchService, IUserRepository userRepository)
        {
            this.branchService = branchService;
            this.userRepository = userRepository;
        }

        [Route("user-branches")]
        [HttpGet]
        public HttpResponseMessage UserBranches()
        {
            var userBranches = this.branchService.GetUserBranchesFriendlyInformation(this.UserName);

            return this.Request.CreateResponse(HttpStatusCode.OK, userBranches);
        }

        [Route("create-user-using-current-context")]
        [HttpGet]
        public HttpResponseMessage CreateUserUsingCurrentContext()
        {
            try
            {
                var user = new User {Name = Thread.CurrentPrincipal.Identity.Name};

                this.userRepository.CurrentUser = user.Name;

                this.userRepository.Save(user);

                return this.Request.CreateResponse(HttpStatusCode.Created, user);
            }
            catch (Exception exception)
            {
                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        [Route("users/{name}")]
        [HttpGet]
        public HttpResponseMessage Users(string name)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
