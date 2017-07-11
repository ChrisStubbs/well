﻿namespace PH.Well.Services
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Transactions;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using static PH.Well.Domain.Mappers.AutoMapperConfig;

    public class ManualCompletionService : IManualCompletionService
    {
        private readonly IJobService jobService;
        private readonly IEpodUpdateService epodUpdateService;

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

        public ManualCompletionService(IJobService jobService, IEpodUpdateService epodUpdateService)
        {
            this.jobService = jobService;
            this.epodUpdateService = epodUpdateService;
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
            List<Job> invoicedJobs = GetInvoicedJobsWithRoute(jobIds);
            actionJobs(invoicedJobs);

            List<Job> completedJobs = new List<Job>();

            foreach (var job in invoicedJobs)
            {
                var dto = Mapper.Map<Job, JobDTO>(job);
                job.ResolutionStatus = ResolutionStatus.CompletedByWell;

                using (var transactionScope = new TransactionScope())
                {
                    epodUpdateService.UpdateJob(dto, job, job.JobRoute.BranchId, job.JobRoute.RouteDate);
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

        private List<Job> GetInvoicedJobsWithRoute(IEnumerable<int> jobIds)
        {
            return jobService.GetJobsWithRoute(jobIds).Where(x => x.WellStatus == WellStatus.Invoiced).ToList();
        }

    }
}