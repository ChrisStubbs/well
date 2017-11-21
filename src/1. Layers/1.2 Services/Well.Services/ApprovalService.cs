namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Contracts;
    using Common.Extensions;
    using Contracts;
    using Domain;
    using Domain.ValueObjects;
    using Repositories;
    using Repositories.Contracts;

    public class ApprovalService : IApprovalService
    {
        private readonly ILogger logger;
        private readonly IJobRepository jobRepository;
        private readonly IAssigneeReadRepository assigneeRepository;
        private readonly IDbMultiConfiguration multiDatabases;

        public ApprovalService(ILogger logger, 
            IJobRepository jobRepository, 
            IAssigneeReadRepository assigneeRepository,
            IDbMultiConfiguration multiDatabases
            )
        {
            this.logger = logger;
            this.jobRepository = jobRepository;
            this.assigneeRepository = assigneeRepository;
            this.multiDatabases = multiDatabases;
        }

        public IList<JobToBeApproved> GetJobsToBeApproved(int branchId)
        {
            var jobs = jobRepository.GetJobsToBeApproved(branchId).ToList();
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