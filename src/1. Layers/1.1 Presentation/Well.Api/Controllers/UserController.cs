namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
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
            var userBranches = this.branchService.GetUserBranchesFriendlyInformation(this.UserIdentityName);

            return this.Request.CreateResponse(HttpStatusCode.OK, userBranches);
        }

        [Route("users-for-branch/{branchId}")]
        [HttpGet]
        public HttpResponseMessage UsersForBranch(int branchId)
        {
            var users = this.userRepository.GetByBranchId(branchId);

            return this.Request.CreateResponse(HttpStatusCode.OK, users);
        }
        
        [Route("create-user-using-current-context")]
        [HttpGet]
        public HttpResponseMessage CreateUserUsingCurrentContext()
        {
            try
            {
                var user = this.activeDirectoryService.GetUser(this.UserIdentityName);

                // this method is used via the BDD for setting up test users so we are just defaulting 
                // the threshold level to max for now
                user.ThresholdLevelId = (int)ThresholdLevel.Level1;

                this.userRepository.CurrentUser = this.UserIdentityName;

                this.userRepository.Save(user);

                return this.Request.CreateResponse(HttpStatusCode.Created, user);
            }
            catch (Exception exception)
            {
                this.logger.LogError($"Error when trying to save user {this.UserIdentityName}", exception);

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
        public HttpResponseMessage Assign(UserJob userJob)
        {
            try
            {
                if (userJob.UserId > 0 && userJob.JobId > 0)
                {
                    this.userRepository.CurrentUser = this.UserIdentityName;
                    this.userRepository.AssignJobToUser(userJob.UserId, userJob.JobId);

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

        [Route("unassign-user-from-job")]
        [HttpPost]
        public HttpResponseMessage UnAssign(int jobId)
        {
            try
            {
                this.userRepository.CurrentUser = this.UserIdentityName;
                this.userRepository.UnAssignJobToUser(jobId);

                return this.Request.CreateResponse(HttpStatusCode.Created, new { success = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when trying to unassign the user from the job", exception);
                return this.Request.CreateResponse(HttpStatusCode.OK, new { failure = true });
            }
        }
    }
}