namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Common.Contracts;
    using Domain.Extensions;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public partial class JobService : IJobService
    {
        private readonly IJobRepository jobRepository;
        private readonly List<Func<Job, ResolutionStatus>> evaluators;
        private readonly IUserThresholdService userThresholdService;
        private readonly IDateThresholdService dateThresholdService;
        private readonly Dictionary<ResolutionStatus, Func<Job, ResolutionStatus>> steps;
        private readonly IAssigneeReadRepository assigneeReadRepository;
        private readonly ILineItemSearchReadRepository lineItemRepository;
        private readonly IUserNameProvider userNameProvider;
        private readonly IUserRepository userRepository;

        public JobService(IJobRepository jobRepository,
            IUserThresholdService userThresholdService,
            IDateThresholdService dateThresholdService,
            IAssigneeReadRepository assigneeReadRepository,
            ILineItemSearchReadRepository lineItemRepository,
            IUserNameProvider userNameProvider,
            IUserRepository userRepository
            )
        {
            this.jobRepository = jobRepository;
            this.evaluators = new List<Func<Job, ResolutionStatus>>();
            this.steps = new Dictionary<ResolutionStatus, Func<Job, ResolutionStatus>>();
            this.userThresholdService = userThresholdService;
            this.dateThresholdService = dateThresholdService;
            this.assigneeReadRepository = assigneeReadRepository;
            this.lineItemRepository = lineItemRepository;
            this.userNameProvider = userNameProvider;
            this.userRepository = userRepository;
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

            if (job.PerformanceStatus == PerformanceStatus.Abypa
                || job.PerformanceStatus == PerformanceStatus.Nbypa
                || job.PerformanceStatus == PerformanceStatus.Wbypa)
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
            if (!string.IsNullOrWhiteSpace(job.InvoiceNumber) || (string.Equals(job.JobTypeCode.Trim().ToLower(), "upl-glo", StringComparison.OrdinalIgnoreCase)))
            {
                job.JobStatus = JobStatus.InComplete;
            }
        }

        public bool CanEdit(Job job, string userName)
        {
            return job.ResolutionStatus.IsEditable()
                    && IsJobAssignedToUser(job, userName)
                   && job.JobTypeEnumValue != JobType.GlobalUplift;
        }

        public bool CanManuallyComplete(Job job, string userName)
        {
            return (job.WellStatus == WellStatus.Invoiced || job.JobStatus == JobStatus.CompletedOnPaper) 
                && IsJobAssignedToUser(job,userName)
                && job.JobTypeEnumValue != JobType.GlobalUplift;
        }

        private bool IsJobAssignedToUser(Job job, string userName)
        {
            return userName.Equals(assigneeReadRepository.GetByJobId(job.Id)?.IdentityName,
                StringComparison.OrdinalIgnoreCase);
        }

        public void SetGrn(int jobId, string grn)
        {
            var job = GetJobsWithRoute(new[] { jobId }).FirstOrDefault();
            if (job != null)
            {
                var jobRoute = job.JobRoute;
                var earliestSubmitDate = dateThresholdService.GracePeriodEnd(
                                                    jobRoute.RouteDate, 
                                                    jobRoute.BranchId, 
                                                    job.GetRoyaltyCode());

                if (earliestSubmitDate < DateTime.Now)
                {
                    throw new Exception("GRN can no longer be modified");
                }

                jobRepository.SaveGrn(jobId, grn);
            }

        }

        #endregion

        #region IJobResolutionStatus

        private ResolutionStatus AfterCompletionStep(ResolutionStatus currentCompletionStatus, Job job)
        {
            // ProofOfDelivery jobs shouldn't be set to ActionRequired
            if (!job.IsProofOfDelivery && job.LineItems.SelectMany(p => p.LineItemActions).Any(lia => lia.Quantity > 0 && lia.DeliveryAction == DeliveryAction.NotDefined))
            {
                return ResolutionStatus.ActionRequired;
            }

            if (dateThresholdService.GracePeriodEnd(job.JobRoute.RouteDate, job.JobRoute.BranchId, job.GetRoyaltyCode()) < DateTime.Now)
            {
                return ResolutionStatus.Closed | currentCompletionStatus;
            }

            return currentCompletionStatus;
        }

        private void fillSteps()
        {
            steps.Add(ResolutionStatus.Imported, job => ResolutionStatus.DriverCompleted);

            steps.Add(ResolutionStatus.DriverCompleted, job => AfterCompletionStep(ResolutionStatus.DriverCompleted, job));

            steps.Add(ResolutionStatus.ManuallyCompleted, job => AfterCompletionStep(ResolutionStatus.ManuallyCompleted, job));

            steps.Add(ResolutionStatus.ActionRequired, GetCurrentResolutionStatus);

            steps.Add(ResolutionStatus.PendingSubmission, job =>
            {

                if (this.userThresholdService.UserHasRequiredCreditThreshold(job))
                {
                    return ResolutionStatus.Approved;
                }

                return ResolutionStatus.PendingApproval;
            });

            steps.Add(ResolutionStatus.PendingApproval, job =>
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
                if (dateThresholdService.GracePeriodEnd(job.JobRoute.RouteDate, job.JobRoute.BranchId, job.GetRoyaltyCode()) < DateTime.Now)
                {
                    return ResolutionStatus.Closed | ResolutionStatus.Credited;
                }

                return ResolutionStatus.Credited;
            });

            steps.Add(ResolutionStatus.Resolved, job =>
            {
                if (dateThresholdService.GracePeriodEnd(job.JobRoute.RouteDate, job.JobRoute.BranchId, job.GetRoyaltyCode()) < DateTime.Now)
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

                if (actions.Any() && (job.ResolutionStatus <= ResolutionStatus.PendingSubmission || job.ResolutionStatus == ResolutionStatus.ManuallyCompleted))
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

        #endregion

        public IEnumerable<Job> PopulateLineItemsAndRoute(IEnumerable<Job> jobs)
        {
            var jobList = jobs.ToList();
            var lineItems = lineItemRepository.GetLineItemByJobIds(jobList.Select(x => x.Id));

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
            return PopulateLineItemsAndRoute(new[] { job }).First();
        }

        public IEnumerable<Job> GetJobsWithRoute(IEnumerable<int> jobIds)
        {
            var jobs = jobRepository.GetByIds(jobIds).ToList();
            var jobRoutes = jobRepository.GetJobsRoute(jobs.Select(x => x.Id));
            jobs.ForEach(job =>
                {
                    job.JobRoute = jobRoutes.Single(x => x.JobId == job.Id);
                }
            );
            return jobs;
        }

        public IEnumerable<int> GetJobsIdsAssignedToCurrentUser(IEnumerable<int> jobIds)
        {
            var username = this.userNameProvider.GetUserName();
            var user = this.userRepository.GetByIdentity(username);
            return userRepository.GetUserJobsByJobIds(jobIds)
                .Where(x => x.UserId == user.Id).Select(x => x.JobId);
        }

    }
}
