namespace PH.Well.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using PH.Well.Common.Contracts;
    using PH.Well.Repositories.Contracts;

    public class BranchController : ApiController
    {
        private readonly ILogger logger;

        private readonly IBranchRepository branchRespository;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        public BranchController(
            ILogger logger,
            IBranchRepository branchRepository, 
            IServerErrorResponseHandler serverErrorResponseHandler)
        {
            this.logger = logger;
            this.branchRespository = branchRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
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
    }
}