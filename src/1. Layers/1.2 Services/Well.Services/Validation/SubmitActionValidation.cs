namespace PH.Well.Services.Validation
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


        public SubmitActionValidation(IUserNameProvider userNameProvider,
            IUserRepository userRepository,
            IDateThresholdService dateThresholdService)
        {
            this.userNameProvider = userNameProvider;
            this.userRepository = userRepository;
            this.dateThresholdService = dateThresholdService;
        }

        public SubmitActionResult Validate(SubmitActionModel submitAction, IEnumerable<Job> jobs)
        {
            var jobList = jobs.ToList();
            var username = this.userNameProvider.GetUserName();
            var user = this.userRepository.GetByIdentity(username);

            if (user == null)
            {
                return new SubmitActionResult { Message = $"User not found ({username}). Can not submit actions" };
            }

            var userJobs = userRepository.GetUserJobsByJobIds(submitAction.JobIds);

            if (userJobs.Any(x => x.UserId != user.Id))
            {
                return new SubmitActionResult { Message = $"User not assigned to all the items selected can not submit actions" };
            }

            var pendingSubmissionJobs = jobList.Where(x => x.ResolutionStatus == ResolutionStatus.PendingSubmission).ToList();

            if (!pendingSubmissionJobs.Any())
            {
                return new SubmitActionResult { Message = $"There are no jobs 'Pending Submission' for the selected items" };
            }

            var incorrectStateJobs = jobList.Where(x => x.ResolutionStatus != ResolutionStatus.PendingSubmission);
            if (incorrectStateJobs.Any())
            {
                var incorrectStateJobstring = string.Join(",", incorrectStateJobs.Select(x => $"JobId:{x.Id} Invoice:{x.InvoiceNumber} Status: {x.ResolutionStatus} "));
                return new SubmitActionResult { Message = $"Can not submit actions for jobs. The following jobs are not in Pending Submission State {incorrectStateJobstring}." };
            }

            var result = HasEarliestSubmitDateBeenReached(pendingSubmissionJobs);

            if (!result.IsValid)
            {
                return result;
            }

            if (HaveItemsToCredit(jobList))
            {
                result = ValidateUserForCrediting();
            }

            return result;
        }

        public virtual SubmitActionResult HasEarliestSubmitDateBeenReached(IList<Job> unsubmittedJobs)
        {
            var jobRoutes = unsubmittedJobs.Select(x => x.JobRoute);

            var jobsBeforeEarliestSubmitDate = jobRoutes.Where(x => DateTime.Now < dateThresholdService.EarliestSubmitDate(x.RouteDate, x.BranchId)).ToArray();

            if (jobsBeforeEarliestSubmitDate.Any())
            {
                var jobError = string.Join(",", jobsBeforeEarliestSubmitDate.Select(
                    x => $"{x.JobId}: earliest credit date: {dateThresholdService.EarliestSubmitDate(x.RouteDate, x.BranchId)}"
                ).Distinct());

                return new SubmitActionResult { IsValid = false, Message = $"Job nos: '{jobError}' have not reached the earliest credit date so can not be submitted." };
            }

            return new SubmitActionResult { IsValid = true };
        }

        public virtual bool HaveItemsToCredit(IList<Job> jobs)
        {
            return jobs.Any(job => job.GetAllLineItemActions().Any(x => x.DeliveryAction == DeliveryAction.Credit));
        }

        public virtual SubmitActionResult ValidateUserForCrediting()
        {
            var username = this.userNameProvider.GetUserName();
            var user = this.userRepository.GetByIdentity(username);

            if (user == null)
            {
                return new SubmitActionResult { Message = $"User not found ({username})" };
            }

            if (user.ThresholdLevelId == null)
            {
                return new SubmitActionResult { Message = $"You must be assigned a threshold level before crediting." };
            }

            return new SubmitActionResult { IsValid = true };
        }
    }
}
