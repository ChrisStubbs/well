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
        private readonly IPostImportRepository postImportRepository;

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
            IUserNameProvider userNameProvider,
            IPostImportRepository postImportRepository)
        {
            this.jobService = jobService;
            this.epodFileImportCommands = epodFileImportCommands;

            this.lineItemActionRepository = lineItemActionRepository;
            this.userNameProvider = userNameProvider;
            this.postImportRepository = postImportRepository;
        }

        public IEnumerable<Job> CompleteAsBypassed(IEnumerable<int> jobIds)
        {
            return ManuallyCompleteJobs(jobIds, ManualCompletionType.CompleteAsBypassed);
        }

        public IEnumerable<Job> CompleteAsClean(IEnumerable<int> jobIds)
        {
            return ManuallyCompleteJobs(jobIds, ManualCompletionType.CompleteAsClean);
        }

        public IEnumerable<Job> ManuallyCompleteJobs(IEnumerable<int> jobIds, ManualCompletionType completionType)
        {
            List<Job> invoicedJobs = GetJobsAvailableForCompletion(jobIds).ToList();

            switch (completionType)
            {
                case ManualCompletionType.CompleteAsClean:
                    MarkAsComplete(invoicedJobs);
                    break;
                case ManualCompletionType.CompleteAsBypassed:
                    MarkAsBypassed(invoicedJobs);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(completionType));
            }
           
            using (var transactionScope = new TransactionScope())
            {
                foreach (var job in invoicedJobs)
                {
                    job.ResolutionStatus = ResolutionStatus.ManuallyCompleted;


                    // Clear job exceptions
                    lineItemActionRepository.DeleteAllLineItemActionsForJob(job.Id);
                    // Update job
                    epodFileImportCommands.UpdateWithEvents(job, job.JobRoute.BranchId, job.JobRoute.RouteDate);

                    // Create LineItemActions if manually complete standard uplift job or manually bypass non standard uplift job
                    if ((job.JobType == JobType.StandardUplift && completionType == ManualCompletionType.CompleteAsClean)
                        ||
                        (job.JobType != JobType.StandardUplift && completionType == ManualCompletionType.CompleteAsBypassed))
                    {
                        this.postImportRepository.PostTranSendImport(new[] { job.Id });
                    }

                    // Compute well status
                    jobService.ComputeAndPropagateWellStatus(job);
                }

                transactionScope.Complete();
            }
            return invoicedJobs;
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
            return jobService.CanManuallyComplete(job, userNameProvider.GetUserName());
        }
    }
}