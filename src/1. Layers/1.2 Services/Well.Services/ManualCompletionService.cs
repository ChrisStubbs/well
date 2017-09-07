using PH.Well.Common.Contracts;

namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Repositories.Contracts;
    using static PH.Well.Domain.Mappers.AutoMapperConfig;

    public class ManualCompletionService : IManualCompletionService
    {
        private readonly IJobService jobService;
        private readonly IEpodFileImportCommands epodFileImportCommands;
        private readonly ILineItemActionRepository lineItemActionRepository;
        private readonly IUserNameProvider userNameProvider;

        public IEnumerable<Job> Complete(IEnumerable<int> jobIds, ManualCompletionType type)
        {
            switch (type)
            {
                case ManualCompletionType.CompleteAsClean:
                    return CompleteAsClean(jobIds);
                case ManualCompletionType.CompleteAsBypassed:
                    return CompleteAsBypassed(jobIds);
                default:
                    throw new NotImplementedException();
            }
        }

        public ManualCompletionService(
            IJobService jobService,
            IEpodFileImportCommands epodFileImportCommands, 
            ILineItemActionRepository lineItemActionRepository,
            IUserNameProvider userNameProvider)
        {
            this.jobService = jobService;
            this.epodFileImportCommands = epodFileImportCommands;

            this.lineItemActionRepository = lineItemActionRepository;
            this.userNameProvider = userNameProvider;
        }

        public IEnumerable<Job> CompleteAsBypassed(IEnumerable<int> jobIds)
        {
            return ManuallyCompleteJobs(jobIds, MarkAsBypassed);
        }

        public IEnumerable<Job> CompleteAsClean(IEnumerable<int> jobIds)
        {
            return ManuallyCompleteJobs(jobIds, MarkAsComplete);
        }

        public IEnumerable<Job> ManuallyCompleteJobs(IEnumerable<int> jobIds, Action<IEnumerable<Job>> actionJobs)
        {
            List<Job> invoicedJobs = GetJobsAvailableForCompletion(jobIds).ToList();
            actionJobs(invoicedJobs);

            List<Job> completedJobs = new List<Job>();

            foreach (var job in invoicedJobs)
            {
                job.ResolutionStatus = ResolutionStatus.ManuallyCompleted;

                using (var transactionScope = new TransactionScope())
                {
                    lineItemActionRepository.DeleteAllLineItemActionsForJob(job.Id);
                    epodFileImportCommands.UpdateWithEvents(job, job.JobRoute.BranchId, job.JobRoute.RouteDate);
                    completedJobs.AddRange(epodFileImportCommands.RunPostInvoicedProcessing(new List<int> { job.Id }));

                    // Compute well status
                    jobService.ComputeWellStatus(job);

                    transactionScope.Complete();
                }
            }
            return completedJobs;
        }

        private void MarkAsBypassed(IEnumerable<Job> invoicedJobs)
        {
            foreach (var job in invoicedJobs)
            {
                job.PerformanceStatus = PerformanceStatus.Wbypa;
            }
        }

        private void MarkAsComplete(IEnumerable<Job> invoicedJobs)
        {
            foreach (var job in invoicedJobs)
            {
                job.JobStatus = JobStatus.Clean;
            }
        }

        public IEnumerable<Job> GetJobsAvailableForCompletion(IEnumerable<int> jobIds)
        {
            IEnumerable<int> userJobsIds = jobService.GetJobsIdsAssignedToCurrentUser(jobIds);
            return jobService.GetJobsWithRoute(userJobsIds)
                .Where(CanManuallyComplete)
                .ToList();
        }

        public bool CanManuallyComplete(Job job)
        {
            return  jobService.CanManuallyComplete(job, userNameProvider.GetUserName());
        }
    }
}