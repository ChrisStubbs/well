namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Models;
    using PH.Well.Api.Mapper;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class BranchController : BaseApiController
    {
        private readonly ILogger logger;

        private readonly IBranchRepository branchRespository;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        private readonly IBranchService branchService;

        private readonly IBranchModelMapper branchModelMapper;

        public BranchController(
            ILogger logger,
            IBranchRepository branchRepository, 
            IServerErrorResponseHandler serverErrorResponseHandler,
            IBranchService branchService,
            IBranchModelMapper branchModelMapper)
        {
            this.logger = logger;
            this.branchRespository = branchRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.branchService = branchService;
            this.branchModelMapper = branchModelMapper;
        }

        [HttpGet]
        public HttpResponseMessage Get(string username = null)
        {
            try
            {
                var branches = this.branchRespository.GetAll();

                if (branches.Any())
                {
                    var userBranches = this.branchRespository.GetBranchesForUser(string.IsNullOrWhiteSpace(username) ? this.UserIdentityName : username.Replace('-', ' '));

                    IEnumerable<BranchModel> model = this.branchModelMapper.Map(branches, userBranches);

                    return this.Request.CreateResponse(HttpStatusCode.OK, model);
                }

                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                this.logger.LogError("An error occcured when getting branches!", ex);
                return this.serverErrorResponseHandler.HandleException(Request, ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage Post(Branch[] branches)
        {
            try
            {
                if (branches.Length > 0)
                {
                    this.branchService.SaveBranchesForUser(branches, this.UserIdentityName);
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

        [Route("save-branches-on-behalf-of-user")]
        [HttpPost]
        public HttpResponseMessage Post(Branch[] branches, string username, string domain)
        {
            try
            {
                if (branches.Length > 0)
                {
                    this.branchService.SaveBranchesOnBehalfOfAUser(branches, username, this.UserIdentityName, domain);
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
    }
}