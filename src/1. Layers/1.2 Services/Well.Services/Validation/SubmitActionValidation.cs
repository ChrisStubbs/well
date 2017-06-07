namespace PH.Well.Services.Validation
{
    using Common.Contracts;
    using Contracts;
    using Domain.ValueObjects;
    using Repositories.Contracts;
    using System.Collections.Generic;
    using System.Linq;

    public class SubmitActionValidation : ISubmitActionValidation
    {
        private readonly IUserNameProvider userNameProvider;
        private readonly IUserRepository userRepository;
        private readonly ISubmitCreditActionValidation creditActionValidation;

        public SubmitActionValidation(IUserNameProvider userNameProvider,
            IUserRepository userRepository,
            ISubmitCreditActionValidation creditActionValidation)
        {
            this.userNameProvider = userNameProvider;
            this.userRepository = userRepository;
            this.creditActionValidation = creditActionValidation;
        }

        public SubmitActionResult Validate(SubmitActionModel submitAction, IEnumerable<LineItemActionSubmitModel> allUnsubmittedItems)
        {
            
            var username = this.userNameProvider.GetUserName();
            var user = this.userRepository.GetByIdentity(username);

            if (user == null)
            {
                return new SubmitActionResult { Message = $"User not found ({username}). Can not submit action" };
            }

            var userJobs = userRepository.GetUserJobsByJobIds(submitAction.JobIds);

            if (userJobs.Any(x => x.UserId != user.Id))
            {
                return new SubmitActionResult { Message = $"User not assigned to all the items selected can not submit '{submitAction.Action}' action" };
            }

            if (!submitAction.ItemsToSubmit.Any())
            {
                return new SubmitActionResult { Message = $"There are no '{submitAction.Action}' actions for the selected items" };
            }

            if (submitAction.ItemsToSubmit.Any(x => x.SubmittedDate.HasValue))
            {
                var submittedJobIds = string.Join(",", submitAction.ItemsToSubmit.Where(x => x.SubmittedDate.HasValue).Select(x => x.JobId));
                return new SubmitActionResult { Message = $"Can not submit {submitAction.Action} action for jobs {submittedJobIds} as these have already been submitted." };
            }

            return creditActionValidation.Validate(submitAction, allUnsubmittedItems);

        }
    }
}
