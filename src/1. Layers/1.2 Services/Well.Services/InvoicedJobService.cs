namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using static PH.Well.Domain.Mappers.AutoMapperConfig;

    public class InvoicedJobService
    {
        private readonly IJobService jobService;
        private readonly IEpodUpdateService epodUpdateService;

        public InvoicedJobService(IJobService jobService, IEpodUpdateService epodUpdateService)
        {
            this.jobService = jobService;
            this.epodUpdateService = epodUpdateService;
        }

        public void MarkAsBypassed(IEnumerable<int> jobIds)
        {
            ManuallyCompleteJobs(jobIds, MarkAsBypassed);
        }

        public void MarkAsComplete(IEnumerable<int> jobIds)
        {
            ManuallyCompleteJobs(jobIds, MarkAsComplete);
        }

        public void ManuallyCompleteJobs(IEnumerable<int> jobIds, Action<IEnumerable<Job>> actionJobs)
        {
            List<Job> invoicedJobs = GetInvoicedJobsWithRoute(jobIds);
            actionJobs(invoicedJobs);

            foreach (var job in invoicedJobs)
            {
                var dto = Mapper.Map<Job, JobDTO>(job);
                job.ResolutionStatus = ResolutionStatus.CompletedByWell;

                using (var transactionScope = new TransactionScope())
                {
                    epodUpdateService.UpdateJob(dto, job, job.JobRoute.BranchId, job.JobRoute.RouteDate);
                    epodUpdateService.RunPostInvoicedProcessing(new List<int> { job.Id });
                    transactionScope.Complete();
                }
            }
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

        private List<Job> GetInvoicedJobsWithRoute(IEnumerable<int> jobIds)
        {
            return jobService.GetJobsWithRoute(jobIds).Where(x => x.WellStatus == WellStatus.Invoiced).ToList();
        }

    }
}