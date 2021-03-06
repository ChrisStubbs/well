﻿namespace PH.Well.Services.Validation
{
    using System;
    using Common.Contracts;
    using Contracts;
    using Domain.ValueObjects;
    using Repositories.Contracts;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Domain.Enums;

    public class SubmitActionValidation : ISubmitActionValidation
    {
        private readonly IUserNameProvider userNameProvider;
        private readonly IUserRepository userRepository;
        private readonly IDateThresholdService dateThresholdService;
        private readonly IJobService jobService;


        public SubmitActionValidation(IUserNameProvider userNameProvider,
            IUserRepository userRepository,
            IDateThresholdService dateThresholdService,
            IJobService jobService)
        {
            this.userNameProvider = userNameProvider;
            this.userRepository = userRepository;
            this.dateThresholdService = dateThresholdService;
            this.jobService = jobService;
        }

        public SubmitActionResult Validate(int[] jobsId, IEnumerable<Job> jobs)
        {
            var jobList = jobs.ToList();
            var username = this.userNameProvider.GetUserName();
            var user = this.userRepository.GetByIdentity(username);

            if (user == null)
            {
                return new SubmitActionResult { Message = $"User not found ({username}). Can not submit exceptions" };
            }

            var userJobs = userRepository.GetUserJobsByJobIds(jobsId);

            if (userJobs.Any(x => x.UserId != user.Id))
            {
                return new SubmitActionResult { Message = "You may only submit jobs which are assigned to you and are Pending Submission. " + 
                                                          "If both criteria are met, it is possible that this job has been re-assigned. " + 
                                                          "Please refresh data and try again."
                };
            }

            var pendingSubmissionJobs = jobList
                .Where(x => x.ResolutionStatus == ResolutionStatus.PendingSubmission || x.ResolutionStatus == ResolutionStatus.PendingApproval
                ).ToList();

            if (!pendingSubmissionJobs.Any())
            {
                return new SubmitActionResult { Message = $"There are no jobs 'Pending Submission' for the selected items" };
            }

            var ids = pendingSubmissionJobs.Select(p => p.Id).ToList();
            var incorrectStateJobs = jobList
                .Where(x => !ids.Contains(x.Id))
                .ToList();

            if (incorrectStateJobs.Any())
            {
                var incorrectStateJobstring = string.Join(",", incorrectStateJobs.Select(x => $"JobId:{x.Id} Invoice:{x.InvoiceNumber} Status: {x.ResolutionStatus} "));
                return new SubmitActionResult
                {
                    Message = $"Can not submit exceptions for jobs. " +
                                                          $"The following jobs are not in Pending Submission / Pending Approval State " +
                                                          $"{incorrectStateJobstring}."
                };
            }
            var result = new SubmitActionResult {IsValid = true};
            HasEarliestSubmitDateBeenReached(pendingSubmissionJobs, result);
            if (!result.IsValid)
            {
                return result;
            }

            ValidateJobsCanBeEdited(pendingSubmissionJobs, result);
            if (!result.IsValid)
            {
                return result;
            }

            if (HaveItemsToCredit(jobList))
            {
                ValidateUserForCrediting(result);
            }

            return result;
        }

        public virtual void HasEarliestSubmitDateBeenReached(IList<Job> unsubmittedJobs, SubmitActionResult result)
        {
            var jobsBeforeEarliestSubmitDate =
                unsubmittedJobs.Where(x => DateTime.Now < dateThresholdService.GracePeriodEnd(
                                               x.JobRoute.RouteDate, x.JobRoute.BranchId, x.GetRoyaltyCode())).ToArray();
            if (jobsBeforeEarliestSubmitDate.Any())
            {
                result.Warnings.Add(
                    @"Please note: Invoice(s) still within grace period. If you continue, any additional credits for this invoice(s) will require manual processing");
            }
        }

        public virtual bool HaveItemsToCredit(IList<Job> jobs)
        {
            return jobs.Any(job => job.GetAllLineItemActions().Any(x => x.DeliveryAction == DeliveryAction.Credit));
        }

        public virtual void ValidateUserForCrediting(SubmitActionResult result)
        {
            var username = this.userNameProvider.GetUserName();
            var user = this.userRepository.GetByIdentity(username);

            if (user == null)
            {
                result.IsValid = false;
                result.Message = $"User not found ({username})";
                return;
            }

            if (!user.CreditThresholdId.HasValue)
            {
                result.IsValid = false;
                result.Message = $"You must be assigned a threshold level before crediting.";
            }
        }

        public virtual void ValidateJobsCanBeEdited(IEnumerable<Job> jobs, SubmitActionResult result)
        {
            var user = userNameProvider.GetUserName();
            var nonEditableJobs = new List<Job>();
            foreach (var job in jobs)
            {
                if (!string.IsNullOrEmpty(jobService.CanEdit(job, user)))
                {
                    result.IsValid = false;
                    nonEditableJobs.Add(job);
                }
            }

            if (!result.IsValid)
            {
                result.Message =
                    "Can not submit exceptions for jobs. Following jobs are not editable " +
                    $"{string.Join(",", nonEditableJobs.Select(x => $"JobId:{x.Id} Invoice:{x.InvoiceNumber} Status: {x.ResolutionStatus} "))}";
            }
        }
    }
}
