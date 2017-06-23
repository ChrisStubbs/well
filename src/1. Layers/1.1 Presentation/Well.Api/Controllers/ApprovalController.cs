using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PH.Well.Api.Controllers
{
    using Domain.Enums;
    using Mapper.Contracts;
    using Models;
    using Repositories.Contracts;
    using Services.Contracts;

    public class ApprovalController : ApiController
    {
        private readonly IJobRepository jobRepository;
        private readonly IJobService jobService;
        private readonly IApprovalMapper mapper;
        private readonly IAssigneeReadRepository assigneeRepository;

        public ApprovalController(
            IJobRepository jobRepository, 
            IJobService jobService,
            IApprovalMapper mapper,
            IAssigneeReadRepository assigneeRepository
            )
        {
            this.jobRepository = jobRepository;
            this.jobService = jobService;
            this.mapper = mapper;
            this.assigneeRepository = assigneeRepository;
        }
        public IEnumerable<ApprovalModel> Get()
        {
            var jobs = jobRepository.GetJobsByResolutionStatus(ResolutionStatus.PendingApproval).ToArray();
            jobs = jobService.PopulateLineItemsAndRoute(jobs).ToArray();
            var assignees = assigneeRepository.GetByJobIds(jobs.Select(x => x.Id));
            if (!jobs.Any())
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            return mapper.Map(jobs, assignees);
        }
    }
}
