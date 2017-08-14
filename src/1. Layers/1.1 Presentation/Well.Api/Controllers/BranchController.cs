﻿using PH.Well.Services;

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

    [Authorize]
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
        public HttpResponseMessage Get(string username = null)
        {
            try
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
            catch (Exception ex)
            {
                return this.serverErrorResponseHandler.HandleException(Request, ex, "An error occurred when getting branches!");
            }
        }

        [HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            var branch = this.branchRespository.GetAllValidBranches().FirstOrDefault(x => x.Id == id);
            if (branch != null)
            {
                var branchArray = new[] {branch};
                return Request.CreateResponse(branchModelMapper.Map(branchArray, branchArray).Single());
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [Route("branch-season")]
        [HttpGet]
        public HttpResponseMessage Get(int seasonalDateId)
        {
            try
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
            catch (Exception ex)
            {
                return this.serverErrorResponseHandler.HandleException(Request, ex, "An error occurred when getting branches!");
            }
        }

        [Authorize(Roles = SecurityPermissions.BranchSelection)]
        [HttpPost]
        public HttpResponseMessage Post(Branch[] branches)
        {
            try
            {
                if (branches.Length > 0)
                {
                    this.branchService.SaveBranchesForUser(branches);
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

        [Authorize(Roles = SecurityPermissions.UserBranchPreferences)]
        [Route("save-branches-on-behalf-of-user")]
        [HttpPost]
        public HttpResponseMessage Post(Branch[] branches, string username, string domain)
        {
            try
            {
                if (branches.Length > 0)
                {
                    this.branchService.SaveBranchesOnBehalfOfAUser(branches, username, domain);
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

        [HttpGet]
        [Route("branchDateThreshold")]
        public IEnumerable<BranchDateThresholdModel> GetBranchDateThresholds()
        {
            return dateThresholdService.GetAll().Select(branchModelMapper.MapDateThreshold).ToList();
        }

        [HttpPost]
        [Route("updateBranchDateThreshold")]
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