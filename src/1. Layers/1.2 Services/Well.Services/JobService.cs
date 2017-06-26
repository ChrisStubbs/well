namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public partial class JobService : IJobService, IJobResolutionStatus
    {
        private readonly IJobRepository jobRepository;
        private readonly List<Func<Job, ResolutionStatus>> evaluators;
        private readonly IUserThresholdService userThresholdService;
        private readonly IDateThresholdService dateThresholdService;
        private readonly Dictionary<ResolutionStatus, Func<Job, ResolutionStatus>> steps;
        private readonly IAssigneeReadRepository assigneeReadRepository;
        private readonly ILineItemSearchReadRepository lineItemRepository;

        public JobService(IJobRepository jobRepository, 
            IUserThresholdService userThresholdService, 
            IDateThresholdService dateThresholdService,
            IAssigneeReadRepository assigneeReadRepository,
            ILineItemSearchReadRepository lineItemRepository)
        {
            this.jobRepository = jobRepository;
            this.evaluators = new List<Func<Job, ResolutionStatus>>();
            this.steps = new Dictionary<ResolutionStatus, Func<Job, ResolutionStatus>>();
            this.userThresholdService = userThresholdService;
            this.dateThresholdService = dateThresholdService;
            this.assigneeReadRepository = assigneeReadRepository;
            this.lineItemRepository = lineItemRepository;
        }

        #region IJobService

        public Job DetermineStatus(Job job, int branchId)
        {
            SetIncompleteJobStatus(job);

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

        public void SetInitialJobStatus(Job job)
        {
            job.JobStatus = string.Equals(job.JobTypeCode.Trim().ToLower(), "del-doc", StringComparison.OrdinalIgnoreCase)
                ? JobStatus.DocumentDelivery
                : JobStatus.AwaitingInvoice;
        }

        public void SetIncompleteJobStatus(Job job)
        {
            //  if (job.JobStatus == JobStatus.AwaitingInvoice && !string.IsNullOrWhiteSpace(job.InvoiceNumber))
            if (!string.IsNullOrWhiteSpace(job.InvoiceNumber)  || (string.Equals(job.JobTypeCode.Trim().ToLower(), "upl-glo", StringComparison.OrdinalIgnoreCase)))
            {
                job.JobStatus = JobStatus.InComplete;
            }
        }

        public bool CanEditActions(Job job, string userName)
        {
            var editableStatuses = new List<ResolutionStatus>
                {
                   ResolutionStatus.DriverCompleted,
                   ResolutionStatus.ActionRequired,
                   ResolutionStatus.PendingSubmission,
                   ResolutionStatus.PendingApproval,
                };

            return editableStatuses.Select(x => x.Value).Contains(job.ResolutionStatus.Value)
                && userName.Equals(assigneeReadRepository.GetByJobId(job.Id)?.IdentityName, StringComparison.OrdinalIgnoreCase);
        }

        public void SetGrn(int jobId, string grn)
        {
            var jobRoute = jobRepository.GetJobRoute(jobId);
            var earliestSubmitDate = dateThresholdService.EarliestSubmitDate(jobRoute.RouteDate, jobRoute.BranchId);
            if (earliestSubmitDate < DateTime.Now)
            {
                throw new Exception("GRN can no longer be modified");
            }

            jobRepository.SaveGrn(jobId, grn);
        }

        #endregion

        #region IJobResolutionStatus

        private void fillSteps()
        {
            steps.Add(ResolutionStatus.Imported, job => ResolutionStatus.DriverCompleted);

            steps.Add(ResolutionStatus.DriverCompleted, job =>
            {
                if (job.LineItems.SelectMany(p => p.LineItemActions).Any())
                {
                    return ResolutionStatus.ActionRequired;
                }
                else if (dateThresholdService.EarliestSubmitDate(job.JobRoute.RouteDate, job.JobRoute.BranchId) < DateTime.Now)
                {
                    return ResolutionStatus.Closed | ResolutionStatus.DriverCompleted;
                }

                return ResolutionStatus.DriverCompleted;
            });

            steps.Add(ResolutionStatus.ActionRequired, job =>
            {
                return GetCurrentResolutionStatus(job);
            });

            steps.Add(ResolutionStatus.PendingSubmission, job =>
            {

                if (this.userThresholdService.UserHasRequiredCreditThreshold(job))
                {
                    return ResolutionStatus.Approved;
                }

                return ResolutionStatus.PendingApproval;
            });

            steps.Add(ResolutionStatus.Approved, job =>
            {
                if (job.LineItems.SelectMany(p => p.LineItemActions).Any(p => p.DeliveryAction == DeliveryAction.Credit))
                {
                    return ResolutionStatus.Credited;
                }

                return ResolutionStatus.Resolved;
            });

            steps.Add(ResolutionStatus.Credited, job =>
            {
                if (dateThresholdService.EarliestSubmitDate(job.JobRoute.RouteDate, job.JobRoute.BranchId) < DateTime.Now)
                {
                    return ResolutionStatus.Closed | ResolutionStatus.Credited;
                }

                return ResolutionStatus.Credited;
            });

            steps.Add(ResolutionStatus.Resolved, job =>
            {
                if (dateThresholdService.EarliestSubmitDate(job.JobRoute.RouteDate, job.JobRoute.BranchId) < DateTime.Now)
                {
                    return ResolutionStatus.Closed | ResolutionStatus.Resolved;
                }

                return ResolutionStatus.Resolved;
            });
        }

        private void fillEvaluators()
        {
            //DriverCompleted
            this.evaluators.Add(job =>
            {
                if (!job.LineItems.SelectMany(p => p.LineItemActions).Any())
                    return ResolutionStatus.DriverCompleted;

                return null;
            });

            //ActionRequired
            this.evaluators.Add(job =>
            {
                var actions = job.LineItems.SelectMany(p => p.LineItemActions).ToList();

                if (actions.Any())
                {
                    if (actions.Any(p => p.DeliveryAction == DeliveryAction.NotDefined))
                    {
                        return ResolutionStatus.ActionRequired;
                    }
                }

                return null;
            });

            //PendingSubmission
            this.evaluators.Add(job =>
            {
                var actions = job.LineItems.SelectMany(p => p.LineItemActions).ToList();

                if (actions.Any() && job.ResolutionStatus <= ResolutionStatus.PendingSubmission)
                {
                    if (actions.All(p => p.DeliveryAction != DeliveryAction.NotDefined))
                    {
                        return ResolutionStatus.PendingSubmission;
                    }
                }

                return null;
            });

            //PendingApproval
            this.evaluators.Add(job =>
            {
                var actions = job.LineItems.SelectMany(p => p.LineItemActions).ToList();

                if (actions.Any() && job.ResolutionStatus == ResolutionStatus.PendingApproval)
                {
                    if (actions.All(p => p.DeliveryAction != DeliveryAction.NotDefined))
                    {
                        return ResolutionStatus.PendingApproval;
                    }
                }

                return null;
            });

            //Approved
            this.evaluators.Add(job =>
            {
                var actions = job.LineItems.SelectMany(p => p.LineItemActions).ToList();

                if (actions.Any() && job.ResolutionStatus == ResolutionStatus.Approved)
                {
                    if (actions.All(p => p.DeliveryAction != DeliveryAction.NotDefined))
                    {
                        return ResolutionStatus.Approved;
                    }
                }

                return null;
            });

            //Credited
            this.evaluators.Add(job =>
            {
                var actions = job.LineItems.SelectMany(p => p.LineItemActions).ToList();

                if (actions.Any() && job.ResolutionStatus == ResolutionStatus.Credited)
                {
                    if (actions.All(p => p.DeliveryAction != DeliveryAction.NotDefined) && actions.Any(p => p.DeliveryAction == DeliveryAction.Credit))
                    {
                        return ResolutionStatus.Credited;
                    }
                }

                return null;
            });

            //Resolved
            this.evaluators.Add(job =>
            {
                var actions = job.LineItems.SelectMany(p => p.LineItemActions).ToList();

                if (actions.Any() && job.ResolutionStatus == ResolutionStatus.Resolved)
                {
                    if (actions.All(p => p.DeliveryAction == DeliveryAction.Close))
                    {
                        return ResolutionStatus.Resolved;
                    }
                }

                return null;
            });

        }

        private List<Func<Job, ResolutionStatus>> Evaluators
        {
            get
            {
                if (this.evaluators.Count == 0)
                {
                    this.fillEvaluators();
                }

                return this.evaluators;
            }
        }

        private Dictionary<ResolutionStatus, Func<Job, ResolutionStatus>> Steps
        {
            get
            {
                if (this.steps.Count == 0)
                {
                    this.fillSteps();
                }

                return this.steps;
            }
        }

        public ResolutionStatus GetCurrentResolutionStatus(Job job)
        {
            return this.Evaluators
                .Select(p => p(job))
                .FirstOrDefault(p => p != null) ?? ResolutionStatus.Invalid;
        }

        public ResolutionStatus GetNextResolutionStatus(Job job)
        {
            if (this.Steps.ContainsKey(job.ResolutionStatus))
            {
                return this.Steps[job.ResolutionStatus](job);
            }

            return ResolutionStatus.Invalid;
        }

        public IEnumerable<Job> PopulateLineItemsAndRoute(IEnumerable<Job> jobs)
        {
            var jobList = jobs.ToList();
            var lineItems = lineItemRepository.GetLineItemByJobIds(jobList.Select(x=> x.Id));
            var jobRoutes = jobRepository.GetJobsRoute(jobList.Select(x => x.Id));

            jobList.ForEach(job =>
                {
                    job.LineItems = lineItems.Where(x => x.JobId == job.Id).ToList();
                    job.JobRoute = jobRoutes.Single(x => x.JobId == job.Id);
                }
            );

            return jobList;
        }

        public Job PopulateLineItemsAndRoute(Job job)
        {
            return PopulateLineItemsAndRoute(new[] {job}).First();
        }

        #endregion
    }
}
