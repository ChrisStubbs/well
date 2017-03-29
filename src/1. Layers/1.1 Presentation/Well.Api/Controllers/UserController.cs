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
    using Validators;

    public class UserController : BaseApiController
    {
        private readonly IBranchService branchService;
        private readonly IUserRepository userRepository;
        private readonly ILogger logger;
        private readonly IJobRepository jobRepository;
        private readonly IActiveDirectoryService activeDirectoryService;

        public UserController(IBranchService branchService, IActiveDirectoryService activeDirectoryService,
            IUserRepository userRepository, ILogger logger,
            IUserNameProvider userNameProvider,
            IJobRepository jobRepository)
            : base(userNameProvider)
        {
            this.branchService = branchService;
            this.userRepository = userRepository;
            this.logger = logger;
            this.jobRepository = jobRepository;
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
            return CreateUser(UserIdentityName);
        }

        [Route("create-user")]
        [HttpPost]
        public HttpResponseMessage CreateUser(string userIdentity)
        {
            try
            {
                var user = this.activeDirectoryService.GetUser(userIdentity);

                // this method is used via the BDD for setting up test users so we are just defaulting 
                // the threshold level to max for now
                user.ThresholdLevelId = (int)ThresholdLevel.Level1;

                ////// DIJ - Why are we changing current user?
                //////this.userRepository.CurrentUser = userIdentity;

                this.userRepository.Save(user);

                return this.Request.CreateResponse(HttpStatusCode.Created, user);
            }
            catch (Exception exception)
            {
                this.logger.LogError($"Error when trying to save user {userIdentity}", exception);

                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [Route("users/{name}")]
        [PHAuthorize(Permissions = Consts.Security.PermissionWellAdmin)]
        [HttpGet]
        public HttpResponseMessage Users(string name)
        {
            var users =
                this.activeDirectoryService.FindUsers(name.Trim(), PH.Well.Api.Configuration.DomainsToSearch).ToList();

            if (!users.Any()) users.Add(new User { Id = -1, Name = "No users found!" });

            return this.Request.CreateResponse(HttpStatusCode.OK, users.OrderBy(x => x.Name).ToList());
        }


        [Route("user/{name}")]
        [PHAuthorize(Permissions = Consts.Security.PermissionWellAdmin)]
        [HttpGet]
        public HttpResponseMessage UserByName(string name)
        {
            try
            {
                var user = userRepository.GetByName(name);
                return this.Request.CreateResponse(HttpStatusCode.OK, user);
            }
            catch (Exception exception)
            {
                this.logger.LogError($"Error when trying to get user by name {name}", exception);

                return this.Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }




        [Route("assign-user-to-job")]
        [HttpPost]
        public HttpResponseMessage Assign(UserJob userJob)
        {
            try
            {
                var user = userRepository.GetById(userJob.UserId);
                var job = jobRepository.GetById(userJob.JobId);
                if (user != null && job != null)
                {
                    this.userRepository.AssignJobToUser(userJob.UserId, userJob.JobId);
                    return this.Request.CreateResponse(HttpStatusCode.Created, new { success = true });
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, new { notAcceptable = true });
            }
            catch (Exception exception)
            {
                this.logger.LogError("Error when trying to assign job for the user", exception);
                return this.Request.CreateResponse(HttpStatusCode.OK, new { failure = true });
            }
        }

        [Route("unassign-user-from-job")]
        [HttpPost]
        public HttpResponseMessage UnAssign(int jobId)
        {
            try
            {
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