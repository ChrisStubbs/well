﻿using PH.Well.Domain.ValueObjects;

namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
        private readonly Dictionary<ResolutionStatus, Func<Job, ResolutionStatus>> stepsForward;
        private readonly Dictionary<ResolutionStatus, Func<Job, ResolutionStatus>> stepsBack;
        private readonly IAssigneeReadRepository assigneeReadRepository;
        private readonly ILineItemSearchReadRepository lineItemRepository;
        private readonly IUserNameProvider userNameProvider;
        private readonly IUserRepository userRepository;
        private readonly IStopService stopService;
        private readonly IActivityService activityService;
        private readonly IWellStatusAggregator wellStatusAggregator;

        public JobService(IJobRepository jobRepository,
            IUserThresholdService userThresholdService,
            IDateThresholdService dateThresholdService,
            IAssigneeReadRepository assigneeReadRepository,
            ILineItemSearchReadRepository lineItemRepository,
            IUserNameProvider userNameProvider,
            IUserRepository userRepository,
            IStopService stopService,
            IActivityService activityService,
            IWellStatusAggregator wellStatusAggregator
            )
        {
            this.jobRepository = jobRepository;
            this.evaluators = new List<Func<Job, ResolutionStatus>>();
            this.stepsForward = new Dictionary<ResolutionStatus, Func<Job, ResolutionStatus>>();
            this.stepsBack = new Dictionary<ResolutionStatus, Func<Job, ResolutionStatus>>();
            this.userThresholdService = userThresholdService;
            this.dateThresholdService = dateThresholdService;
            this.assigneeReadRepository = assigneeReadRepository;
            this.lineItemRepository = lineItemRepository;
            this.userNameProvider = userNameProvider;
            this.userRepository = userRepository;
            this.stopService = stopService;
            this.activityService = activityService;
            this.wellStatusAggregator = wellStatusAggregator;
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
            if (!hasException && job.OuterDiscrepancyUpdate)
            {
                hasException = true;
            }

            job.JobStatus = hasException ? JobStatus.Exception : JobStatus.Clean;
            return job;
        }

        public void SetInitialJobStatus(Job job)
        {
            job.JobStatus = (job.JobType == JobType.Documents) ? JobStatus.DocumentDelivery : JobStatus.AwaitingInvoice;
            SetIncompleteJobStatus(job);
        }

        // global uplift never has an invoice number, standard uplift may not have an invoice number
        public void SetIncompleteJobStatus(Job job)
        {
            if (!string.IsNullOrWhiteSpace(job.InvoiceNumber) ||
                (job.JobType == JobType.GlobalUplift || job.JobType == JobType.StandardUplift))
            {
                job.JobStatus = JobStatus.InComplete;
            }
        }

        public string CanEdit(Job job, string userName)
        {
            var result = new StringBuilder();

            if (!job.ResolutionStatus.IsEditable())
            {
                result.AppendLine(ResolutionStatusExtensions.IsNotEditableMessage());
            }

            if (!IsJobAssignedToUser(job, userName))
            {
                result.AppendLine("Job must be assigned to current user.");
            }

            if (job.JobType == JobType.GlobalUplift)
            {
                result.AppendLine("Global Uplift jobs cannot be edited.");
            }

            return result.ToString();
        }

        public bool CanManuallyComplete(Job job, string userName)
        {
            return (job.WellStatus == WellStatus.Invoiced
                || job.WellStatus == WellStatus.Replanned
                || job.JobStatus == JobStatus.CompletedOnPaper
                || job.WellStatus == WellStatus.Bypassed)
                && IsJobAssignedToUser(job, userName);
        }

        private bool IsJobAssignedToUser(Job job, string userName)
        {
            return userName.Equals(assigneeReadRepository.GetByJobId(job.Id)?.IdentityName,
                StringComparison.OrdinalIgnoreCase);
        }

        public bool SetGrn(int jobId, string grn)
        {
            var job = GetJobsWithRoute(new[] { jobId }).FirstOrDefault();
            if (job != null)
            {
                var jobRoute = job.JobRoute;
                var earliestSubmitDate = dateThresholdService.GracePeriodEnd(
                                                    jobRoute.RouteDate,
                                                    jobRoute.BranchId,
                                                    job.GetRoyaltyCode());

                if (earliestSubmitDate < DateTime.Now && !string.IsNullOrEmpty(job.GrnNumber))
                {
                    return false;
                }

                jobRepository.SaveGrn(jobId, grn);
            }

            return true;
        }

        #endregion

        #region IJobResolutionStatus

        private void fillStepsBack()
        {
            stepsBack.Add(ResolutionStatus.PendingApproval, job => ResolutionStatus.ApprovalRejected);
        }

        private void fillStepsForward()
        {
            Func<ResolutionStatus, Job, ResolutionStatus> AfterCompletionStep = (currentCompletionStatus, job) =>
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
            };

            Func<Job, ResolutionStatus, ResolutionStatus> tryToClose = (job, current) =>
            {
                if (dateThresholdService.GracePeriodEnd(job.JobRoute.RouteDate, job.JobRoute.BranchId, job.GetRoyaltyCode()) < DateTime.Now)
                {
                    return ResolutionStatus.Closed | current;
                }

                return current;
            };

            Func<Job, ResolutionStatus, ResolutionStatus> PendingSubmissionApprovalRejected = (job, newStatus) =>
            {
                if (this.userThresholdService.UserHasRequiredCreditThreshold(job)
                    && job.LineItems.Any() //must have lineitems
                    && !job.LineItems.SelectMany(p => p.LineItemActions) //none of the action can be on NotDefined
                        .Any(p => p.DeliveryAction == DeliveryAction.NotDefined))
                {
                    return ResolutionStatus.Approved;
                }

                return newStatus;
            };

            stepsForward.Add(ResolutionStatus.Imported, job => ResolutionStatus.DriverCompleted);

            stepsForward.Add(ResolutionStatus.DriverCompleted, job => AfterCompletionStep(ResolutionStatus.DriverCompleted, job));

            stepsForward.Add(ResolutionStatus.ManuallyCompleted, job => AfterCompletionStep(ResolutionStatus.ManuallyCompleted, job));

            stepsForward.Add(ResolutionStatus.ActionRequired, GetCurrentResolutionStatus);

            stepsForward.Add(ResolutionStatus.PendingSubmission, job => PendingSubmissionApprovalRejected(job, ResolutionStatus.PendingApproval));

            stepsForward.Add(ResolutionStatus.ApprovalRejected, job => PendingSubmissionApprovalRejected(job, ResolutionStatus.ApprovalRejected));

            stepsForward.Add(ResolutionStatus.PendingApproval, job =>
            {

                if (this.userThresholdService.UserHasRequiredCreditThreshold(job))
                {
                    return ResolutionStatus.Approved;
                }

                return ResolutionStatus.PendingApproval;
            });

            stepsForward.Add(ResolutionStatus.Approved, job =>
            {
                if (job.LineItems.SelectMany(p => p.LineItemActions).Any(p => p.DeliveryAction == DeliveryAction.Credit))
                {
                    return ResolutionStatus.Credited;
                }

                return ResolutionStatus.Resolved;
            });

            stepsForward.Add(ResolutionStatus.Credited, job => tryToClose(job, ResolutionStatus.Credited));

            stepsForward.Add(ResolutionStatus.Resolved, job => tryToClose(job, ResolutionStatus.Resolved));
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
                return job.LineItems
                    .SelectMany(p => p.LineItemActions)
                    .Where(p => p.DeliveryAction == DeliveryAction.NotDefined)
                    .Select(p => ResolutionStatus.ActionRequired)
                    .FirstOrDefault();
            });

            //PendingSubmission
            this.evaluators.Add(job =>
            {
                var actions = job.LineItems.SelectMany(p => p.LineItemActions).ToList();

                if (actions.Any() && (job.ResolutionStatus <= ResolutionStatus.PendingSubmission
                || job.ResolutionStatus == ResolutionStatus.ManuallyCompleted
                || job.ResolutionStatus == ResolutionStatus.ApprovalRejected
                ))
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

        private Dictionary<ResolutionStatus, Func<Job, ResolutionStatus>> StepsForward
        {
            get
            {
                if (this.stepsForward.Count == 0)
                {
                    this.fillStepsForward();
                }

                return this.stepsForward;
            }
        }

        private Dictionary<ResolutionStatus, Func<Job, ResolutionStatus>> StepsBack
        {
            get
            {
                if (this.stepsBack.Count == 0)
                {
                    this.fillStepsBack();
                }

                return this.stepsBack;
            }
        }

        public ResolutionStatus GetCurrentResolutionStatus(Job job)
        {
            return this.Evaluators
                .Select(p => p(job))
                .FirstOrDefault(p => p != null) ?? ResolutionStatus.Invalid;
        }

        public ResolutionStatus StepForward(Job job)
        {
            if (this.StepsForward.ContainsKey(job.ResolutionStatus))
            {
                return this.StepsForward[job.ResolutionStatus](job);
            }

            return ResolutionStatus.Invalid;
        }

        public ResolutionStatus StepBack(Job job)
        {
            if (this.StepsBack.ContainsKey(job.ResolutionStatus))
            {
                return this.StepsBack[job.ResolutionStatus](job);
            }

            return ResolutionStatus.Invalid;
        }

        public ResolutionStatus TryCloseJob(Job job)
        {
            if (job.ResolutionStatus == ResolutionStatus.Resolved
                || job.ResolutionStatus == ResolutionStatus.Credited)
            {
                return job.ResolutionStatus | ResolutionStatus.Closed;
            }

            return job.ResolutionStatus;
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

        /// <summary>
        /// Helper function to get job with minimum required data to compute well status
        /// </summary>
        /// <param name="jobId"></param>
        /// <exception cref="ArgumentException">When job is not found</exception>
        /// <returns></returns>
        private Job GetJobForWellStatusCalculation(int jobId)
        {
            var job = jobRepository.GetForWellStatusCalculationById(jobId);
            if (job != null)
            {
                return job;
            }

            throw new ArgumentException($"Job not found id : {jobId}", nameof(jobId));
        }

        public bool ComputeWellStatus(int jobId)
        {
            // Get the job specified
            var job = GetJobForWellStatusCalculation(jobId);
            return ComputeWellStatus(job);
        }

        public bool ComputeWellStatus(Job job)
        {
            var status = job.JobStatus.ToWellStatus();
            if (job.WellStatus != status)
            {
                job.WellStatus = status;
                this.jobRepository.UpdateWellStatus(job);

                return true;
            }

            return false;
        }

        public bool ComputeAndPropagateWellStatus(int jobId)
        {
            // Get the job specified
            var job = GetJobForWellStatusCalculation(jobId);
            return ComputeAndPropagateWellStatus(job);
        }

        public bool ComputeAndPropagateWellStatus(Job job)
        {
            if (ComputeWellStatus(job))
            {
                // Propagate to parent job & sibling activity/invoice
                this.stopService.ComputeAndPropagateWellStatus(job.StopId);
                // TODO Implement activity service this.activityService.ComputeAndPropagateWellStatus(job.ActivityId);
                return true;
            }
            return false;
        }

        public AssignJobResult Assign(UserJobs userJobs)
        {
            var user = userRepository.GetById(userJobs.UserId);
            var jobs = jobRepository.GetByIds(userJobs.JobIds).ToArray();

            if (user != null && jobs.Any())
            {
                // Allocate all jobs in list
                if (userJobs.AllocatePendingApprovalJobs)
                {
                    foreach (var job in jobs)
                    {
                        this.userRepository.AssignJobToUser(userJobs.UserId, job.Id);
                    }

                    return new AssignJobResult(true, "Jobs have been assigned");
                }
                // Allocate excluding Pending Approval jobs
                else
                {
                    return AssignExcludingPendingApprovalJobs(jobs, user);
                }
            }

            return new AssignJobResult(false, "Error occurred");
        }

        /// <summary>
        /// Helper method that acts as Strategy and shouldn't be used on its own 
        /// </summary>
        /// <param name="jobs"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private AssignJobResult AssignExcludingPendingApprovalJobs(IEnumerable<Job> jobs, User user)
        {
            var jobsToAssign = jobs.Where(x => x.ResolutionStatus != ResolutionStatus.PendingApproval).ToList();
            if (jobsToAssign.Any())
            {
                foreach (var job in jobsToAssign)
                {
                    this.userRepository.AssignJobToUser(user.Id, job.Id);
                }

                return new AssignJobResult(true, "Jobs have been assigned");
            }
            
            // If code reaches here it means that all jobs in jobs were PendingApproval
            var multipleJobs = jobs.Count() > 1;
            if (multipleJobs)
            {
                return new AssignJobResult(false,
                    "Jobs can not be assigned as they are currently in a status of Pending Approval");
            }
            else
            {
                return new AssignJobResult(false,
                    "Job cannot be assigned as it is currently in a status of Pending Approval");
            }
        }

        public AssignJobResult UnAssign(IEnumerable<int> jobIds)
        {
            foreach (var jobId in jobIds)
            {
                this.userRepository.UnAssignJobToUser(jobId);
            }

            return new AssignJobResult(true, "Jobs have been unassigned");
        }
    }
}
