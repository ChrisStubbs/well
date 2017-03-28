namespace PH.Well.Services
{
    using System;
    using System.Linq;
    using System.Transactions;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class JobStatusService : IJobStatusService
    {
        private readonly IJobRepository jobRepository;

        public JobStatusService(IJobRepository jobRepository)
        {
            this.jobRepository = jobRepository;
        }

        public Job DetermineStatus(Job job, int branchId)
        {
            SetIncompleteStatus(job);

            switch (job.JobStatus)
            {
                case JobStatus.AwaitingInvoice:
                case JobStatus.Resolved:
                case JobStatus.DocumentDelivery:
                case JobStatus.CompletedOnPaper:
                    return job;
            }

            if (!string.IsNullOrWhiteSpace(job.JobByPassReason) && job.JobByPassReason.Trim().ToLower() == "manual delivery")
            {
                job.JobStatus = JobStatus.CompletedOnPaper;
                return job;
            }
            if (job.PerformanceStatus == PerformanceStatus.Abypa || job.PerformanceStatus == PerformanceStatus.Nbypa)
            {
                job.JobStatus = JobStatus.Bypassed;
                return job;
            }


            // Fetch all jobs associated with the current job's invoice and branch
            var jobs = this.jobRepository.GetJobsByBranchAndInvoiceNumber(job.Id, branchId, job.InvoiceNumber).ToList();

            var hasException = false;

            if (jobs.Any())
            {
                // If the delivered QTY > the invoiced QTY is an exception
                var products = jobs.SelectMany(x => x.JobDetails).GroupBy(x => x.PhProductCode);

                foreach (var product in products)
                {
                    // The original QTY is duplicated on all deliveries, so take the first one
                    var originalDespatchQty = product.First().OriginalDespatchQty;

                    // Delivered quantity of this product across all deliveries
                    var deliveryedQty = product.Sum(x => x.DeliveredQty);

                    if (deliveryedQty > originalDespatchQty)
                    {
                        hasException = true;
                        break;
                    }
                }

                // Set all jobs to have an exceptions
                if (hasException)
                {
                    using (var transactionScope = new TransactionScope())
                    {
                        jobs.ForEach(x =>
                        {
                            x.JobStatus = JobStatus.Exception;
                            this.jobRepository.Update(x);
                        });

                        transactionScope.Complete();
                    }
                }
            }

            // Any damages are an exception or any shorts are an exception or outer discrepancy found is an exception
            if (!hasException && (job.JobDetails.Any(x => x.IsClean() == false) || job.OuterDiscrepancyUpdate))
            {
                hasException = true;
            }

            job.JobStatus = hasException ? JobStatus.Exception : JobStatus.Clean;
            return job;
        }

        public void SetInitialStatus(Job job)
        {
            job.JobStatus = string.Equals(job.JobTypeCode.Trim().ToLower(), "del-doc", StringComparison.OrdinalIgnoreCase)
                ? JobStatus.DocumentDelivery
                : JobStatus.AwaitingInvoice;
        }

        public void SetIncompleteStatus(Job job)
        {
            if (job.JobStatus == JobStatus.AwaitingInvoice && !string.IsNullOrWhiteSpace(job.InvoiceNumber))
            {
                job.JobStatus = JobStatus.InComplete;
            }
        }
    }
}
