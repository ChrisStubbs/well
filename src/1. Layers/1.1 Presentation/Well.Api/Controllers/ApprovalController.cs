namespace PH.Well.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Domain.ValueObjects;
    using Common.Extensions;
    using Domain;
    using Repositories.Contracts;
    using Services.Contracts;

    public class ApprovalController : ApiController
    {
        private readonly IJobRepository jobRepository;
        private readonly IAssigneeReadRepository assigneeRepository;

        public ApprovalController(
            IJobRepository jobRepository,
            IAssigneeReadRepository assigneeRepository
            )
        {
            this.jobRepository = jobRepository;
            this.assigneeRepository = assigneeRepository;
        }
        public IEnumerable<JobToBeApproved> Get()
        {
            var jobs = jobRepository.GetJobsToBeApproved();
            var assignees = assigneeRepository.GetByJobIds(jobs.Select(p => p.JobId).Distinct());

            return jobs
                .Select(p =>
                {
                    p.AssignedTo = Assignee.GetDisplayNames(assignees.Where(v => v.JobId == p.JobId)) ?? "Unallocated";
                    p.SubmittedBy = p.SubmittedBy.StripDomain();
                    p.BranchName = Branch.GetBranchName(p.BranchId, p.BranchName);

                    return p;
                })
                .ToList();
        }
    }
}
