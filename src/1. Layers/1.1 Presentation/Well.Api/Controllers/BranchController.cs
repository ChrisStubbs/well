using PH.Well.Services;

namespace PH.Well.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Models;
    using Mapper.Contracts;
    using Common.Contracts;
    using Common.Security;
    using Domain;
    using Repositories.Contracts;
    using Services.Contracts;

    public class BranchController : BaseApiController
    {
        private readonly ILogger logger;

        private readonly IBranchRepository branchRespository;

        private readonly IServerErrorResponseHandler serverErrorResponseHandler;

        private readonly IBranchService branchService;

        private readonly IBranchModelMapper branchModelMapper;
        private readonly IDateThresholdService dateThresholdService;

        public BranchController(
            ILogger logger,
            IBranchRepository branchRepository,
            IServerErrorResponseHandler serverErrorResponseHandler,
            IBranchService branchService,
            IBranchModelMapper branchModelMapper,
            IUserNameProvider userNameProvider,
            IDateThresholdService dateThresholdService)
            : base(userNameProvider)
        {
            this.logger = logger;
            this.branchRespository = branchRepository;
            this.serverErrorResponseHandler = serverErrorResponseHandler;
            this.branchService = branchService;
            this.branchModelMapper = branchModelMapper;
            this.dateThresholdService = dateThresholdService;
        }

        [HttpGet]
        [Route("{branchId:int}/branch")]
        [Route("branch")]
        public HttpResponseMessage Get(string username = null)
        {
            var branches = this.branchRespository.GetAllValidBranches();

            if (branches.Any())
            {
                var userBranches = this.branchRespository.GetBranchesForUser(string.IsNullOrWhiteSpace(username) ? this.UserIdentityName : username.Replace('-', ' '));

                IEnumerable<BranchModel> model = this.branchModelMapper.Map(branches, userBranches);

                return this.Request.CreateResponse(HttpStatusCode.OK, model);
            }

            return this.Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            var branch = this.branchRespository.GetAllValidBranches().FirstOrDefault(x => x.Id == id);
            if (branch != null)
            {
                var branchArray = new[] { branch };
                return Request.CreateResponse(branchModelMapper.Map(branchArray, branchArray).Single());
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [Route("{branchId:int}/branch-season")]
        [HttpGet]
        public HttpResponseMessage Get(int seasonalDateId)
        {
            var branches = this.branchRespository.GetAllValidBranches();

            if (branches.Any())
            {
                var userBranches = this.branchRespository.GetBranchesForSeasonalDate(seasonalDateId);

                IEnumerable<BranchModel> model = this.branchModelMapper.Map(branches, userBranches);

                return this.Request.CreateResponse(HttpStatusCode.OK, model);
            }

            return this.Request.CreateResponse(HttpStatusCode.NotFound);

        }

        [HttpPost]
        [Route("branch")]
        public HttpResponseMessage Post(Branch[] branches)
        {
            if (branches.Length > 0)
            {
                this.branchService.SaveBranchesForUser(branches);
                return this.Request.CreateResponse(HttpStatusCode.Created, new { success = true });
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, new { notAcceptable = true });

        }

        [Route("{branchId:int}/save-branches-on-behalf-of-user")]
        [HttpPost]
        public HttpResponseMessage Post(Branch[] branches, string username, string domain)
        {
            if (branches.Length > 0)
            {
                this.branchService.SaveBranchesOnBehalfOfAUser(branches, username, domain);
                return this.Request.CreateResponse(HttpStatusCode.Created, new { success = true });
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, new { notAcceptable = true });

        }

        [HttpGet]
        [Route("{branchId:int}/branchDateThreshold")]
        public IEnumerable<BranchDateThresholdModel> GetBranchDateThresholds()
        {
            return dateThresholdService.GetAll().Select(branchModelMapper.MapDateThreshold).ToList();
        }

        [HttpPost]
        [Route("{branchId:int}/updateBranchDateThreshold")]
        public void UpdateBranchDateThresholds(BranchDateThresholdModel[] branchDateThresholds)
        {
            var branchThresholds = branchDateThresholds.Select(branchModelMapper.MapDateThreshold);
            foreach (var item in branchThresholds)
            {
                dateThresholdService.Update(item);
            }
        }
    }
}