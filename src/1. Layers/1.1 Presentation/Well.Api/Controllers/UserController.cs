using PH.Well.Api.Models;

namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using WebApi.OutputCache.V2;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;
    using Validators;
    using System.Collections.Generic;

    public class UserController : BaseApiController
    {
        private readonly IBranchService branchService;
        private readonly IUserRepository userRepository;
        private readonly ILogger logger;
        private readonly IJobService jobService;
        private readonly IActiveDirectoryService activeDirectoryService;

        public UserController(IBranchService branchService,
            IActiveDirectoryService activeDirectoryService,
            IUserRepository userRepository, ILogger logger,
            IUserNameProvider userNameProvider,
            IJobService jobService)
            : base(userNameProvider)
        {
            this.branchService = branchService;
            this.userRepository = userRepository;
            this.logger = logger;
            this.jobService = jobService;
            this.activeDirectoryService = activeDirectoryService;
        }

        //note: This will use the default database connection as no branchId prefix is supplied in route
        [Route("user-branches")]
        [HttpGet]
        public HttpResponseMessage UserBranches()
        {
            var userBranches = this.branchService.GetUserBranchesFriendlyInformation(this.UserIdentityName);

            return this.Request.CreateResponse(HttpStatusCode.OK, userBranches);
        }

        [Route("{branch:int}/users-for-branch/{branchId}")]
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        public HttpResponseMessage UsersForBranch(int branchId)
        {
            return this.Request.CreateResponse(HttpStatusCode.OK, this.userRepository.GetByBranchId(branchId));
        }

        public IList<User> Get()
        {
            var data = this.userRepository.Get().ToList();
            var result = new List<User>(data.Count());
            var me = data.FirstOrDefault(p => p.IdentityName == this.UserNameProvider.GetUserName());

            if (me != null)
            {
                result.Add(me);
            }

            result.AddRange(data
                .Where(p => p.IdentityName != this.UserNameProvider.GetUserName())
                .OrderBy(p => p.Name));

            return result;
        }

        [Route("{branchId:int}/create-user-using-current-context")]
        [HttpGet]
        public HttpResponseMessage CreateUserUsingCurrentContext()
        {
            return CreateUser(UserIdentityName);
        }

        [Route("{branchId:int}/create-user")]
        [HttpPost]
        public HttpResponseMessage CreateUser(string userIdentity)
        {
            return this.Request.CreateResponse(HttpStatusCode.Created, this.Save(userIdentity));
        }

        private User Save(string userIdentity)
        {
            var user = this.activeDirectoryService.GetUser(userIdentity);

            this.userRepository.Save(user);

            return user;
        }

        [Route("{branchId:int}/users/{name}")]
        [HttpGet]
        public HttpResponseMessage Users(string name)
        {
            var users =
                this.activeDirectoryService.FindUsers(name.Trim(), Api.Configuration.DomainsToSearch).ToList();

            if (!users.Any()) users.Add(new User { Id = -1, Name = "No users found!" });

            return this.Request.CreateResponse(HttpStatusCode.OK, users.OrderBy(x => x.Name).ToList());
        }


        [Route("{branchId:int}/user/{name}")]
        [HttpGet]
        public User UserByName(string name)
        {
            var user = userRepository.GetByName(name);

            if (user == null)
            {
                user = this.CreateNewUser(name);
            }

            if (user == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            return user;
        }

        /// <summary>
        /// Used when it's been trying set set a threshold for a non existing DB user
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private User CreateNewUser(string name)
        {
            var usr = this.activeDirectoryService.FindUsers(name.Split(' ')[0], Api.Configuration.DomainsToSearch)
                .ToList()
                .FirstOrDefault(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            if (usr == null)
            {
                return null;
            }

            return this.Save(usr.IdentityName);
        }

        [Route("{branchId:int}/assign-user-to-jobs")]
        [HttpPost]
        public AssignJobResult Assign(UserJobs userJobs)
        {
            return jobService.Assign(userJobs);
        }

        [Route("{branchId:int}/unassign-user-from-jobs")]
        [HttpPost]
        public AssignJobResult UnAssign(int[] jobIds)
        {
            return jobService.UnAssign(jobIds);
        }
    }
}