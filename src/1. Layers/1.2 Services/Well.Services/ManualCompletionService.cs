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
        private readonly IEpodUpdateService epodUpdateService;
        private readonly ILineItemActionRepository lineItemActionRepository;

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
            IEpodUpdateService epodUpdateService, 
            ILineItemActionRepository lineItemActionRepository)
        {
            this.jobService = jobService;
            this.epodUpdateService = epodUpdateService;
            this.lineItemActionRepository = lineItemActionRepository;
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
                var dto = Mapper.Map<Job, JobDTO>(job);
                job.ResolutionStatus = ResolutionStatus.ManuallyCompleted;

                using (var transactionScope = new TransactionScope())
                {
                    lineItemActionRepository.DeleteAllLineItemActionsForJob(job.Id);
                    epodUpdateService.UpdateJob(dto, job, job.JobRoute.BranchId, job.JobRoute.RouteDate, false);
                    completedJobs.AddRange(epodUpdateService.RunPostInvoicedProcessing(new List<int> { job.Id }));
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
                .Where(x => x.WellStatus == WellStatus.Invoiced
                || x.JobStatus == JobStatus.CompletedOnPaper)
                .ToList();
        }

    }
}