using PH.Well.Domain.ValueObjects;

namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class UserController : BaseApiController
    {
        private readonly IBranchService branchService;
        private readonly IUserRepository userRepository;

        private readonly ILogger logger;

        private readonly IActiveDirectoryService activeDirectoryService;

        public UserController(IBranchService branchService, IActiveDirectoryService activeDirectoryService,
            IUserRepository userRepository, ILogger logger)
        {
            this.branchService = branchService;
            this.userRepository = userRepository;
            this.logger = logger;
            this.activeDirectoryService = activeDirectoryService;
        }

        [Route("user-branches")]
        [HttpGet]
        public HttpResponseMessage UserBranches()
        {
            var userBranches = this.branchService.GetUserBranchesFriendlyInformation(this.UserName);

            return this.Request.CreateResponse(HttpStatusCode.OK, userBranches);
        }


        [Route("users-for-branch/{branchId}")]
        [HttpGet]
        public HttpResponseMessage UsersForBranch(int branchId)
        {
            var users = this.branchService.GetUsersForBranch(branchId);

            return this.Request.CreateResponse(HttpStatusCode.OK, users);
        }



        [Route("create-user-using-current-context")]
        [HttpGet]
        public HttpResponseMessage CreateUserUsingCurrentContext()
        {
            try
            {
                var user = this.activeDirectoryService.GetUser(this.UserName);

                this.userRepository.CurrentUser = this.UserName;

                this.userRepository.Save(user);

                return this.Request.CreateResponse(HttpStatusCode.Created, user);
            }
            catch (Exception exception)
            {
                this.logger.LogError($"Error when trying to save user {this.UserName}", exception);

                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }

        [Route("users/{name}")]
        [HttpGet]
        public HttpResponseMessage Users(string name)
        {
            var users =
                this.activeDirectoryService.FindUsers(name.Trim(), PH.Well.Api.Configuration.DomainsToSearch).ToList();

            if (!users.Any()) users.Add(new User {Id = -1, Name = "No users found!"});

            return this.Request.CreateResponse(HttpStatusCode.OK, users.OrderBy(x => x.Name).ToList());
        }

        [Route("assign-user-to-job")]
        [HttpPost]
        public HttpResponseMessage Post(UserJob userJob)
        {
            try
            {
                if (userJob.UserId > 0 && userJob.JobId > 0)
                {
                    this.branchService.AssignUserToJob(userJob.UserId, userJob.JobId);
                    return this.Request.CreateResponse(HttpStatusCode.Created, new {success = true});
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new {notAcceptable = true});
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when trying to assign job for the user", exception);
                return this.Request.CreateResponse(HttpStatusCode.OK, new {failure = true});
            }
        }
    }
}




/*
         * 
         * 
         *   [Route("save-branches-on-behalf-of-user")]
        [HttpPost]
        public HttpResponseMessage Post(Branch[] branches, string username, string domain)
        {
            try
            {
                if (branches.Length > 0)
                {
                    this.branchService.SaveBranchesOnBehalfOfAUser(branches, username, this.UserName, domain);
                    return this.Request.CreateResponse(HttpStatusCode.Created, new { success = true });
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new { notAcceptable = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when trying to save branches for the user", exception);
                return this.Request.CreateResponse(HttpStatusCode.OK, new { failure = true });
            }
        }
         
        [HttpPost]
        public HttpResponseMessage Post(Branch[] branches)
        {
            try
            {
                if (branches.Length > 0)
                {
                    this.branchService.SaveBranchesForUser(branches, this.UserName);
                    return this.Request.CreateResponse(HttpStatusCode.Created, new { success = true });
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new { notAcceptable = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when trying to save branches for the user", exception);
                return this.Request.CreateResponse(HttpStatusCode.OK, new { failure = true });
            }
        }
         
         
         
         */

   

