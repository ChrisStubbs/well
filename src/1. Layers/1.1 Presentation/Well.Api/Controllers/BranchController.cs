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

    public class BranchController : BaseApiController
    {
        private readonly ILogger logger;

        private readonly IBranchRepository branchRespository;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        private readonly IBranchService branchService;

        public BranchController(
            ILogger logger,
            IBranchRepository branchRepository, 
            IServerErrorResponseHandler serverErrorResponseHandler,
            IBranchService branchService)
        {
            this.logger = logger;
            this.branchRespository = branchRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.branchService = branchService;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                var branches = this.branchRespository.GetAll();

                return !branches.Any()
                    ? this.Request.CreateResponse(HttpStatusCode.NotFound)
                    : this.Request.CreateResponse(HttpStatusCode.OK, branches);
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
    }
}