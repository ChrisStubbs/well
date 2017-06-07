namespace PH.Well.Services.Validation
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Contracts;
    using Contracts;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class SubmitCreditActionValidation : ISubmitCreditActionValidation
    {
        private readonly IUserNameProvider userNameProvider;
        private readonly IUserRepository userRepository;

        public SubmitCreditActionValidation(
            IUserNameProvider userNameProvider,
            IUserRepository userRepository)
        {
            this.userNameProvider = userNameProvider;
            this.userRepository = userRepository;
        }

        public SubmitActionResult Validate(SubmitActionModel submitAction, IEnumerable<LineItemActionSubmitModel> allUnsubmittedItems)
        {
            var result = new SubmitActionResult { IsValid = true };

            if (submitAction.Action == DeliveryAction.Credit)
            {
                result = ValidateUserForCrediting();
                if (result.IsValid)
                {
                    result = AllCreditItemsForInvoiceIncluded(submitAction, allUnsubmittedItems);
                }
            }

            return result;
        }

        public SubmitActionResult AllCreditItemsForInvoiceIncluded(SubmitActionModel submitAction, IEnumerable<LineItemActionSubmitModel> allUnsubmittedItems)
        {
            allUnsubmittedItems = allUnsubmittedItems.ToArray();
            submitAction.SetItemsToSubmit(allUnsubmittedItems);

            var submitInvoiceNos = submitAction.ItemsToSubmit.Select(x => x.InvoiceNumber).Distinct();
            var submitJobIds = submitAction.ItemsToSubmit.Select(x => x.JobId).Distinct();

            var allInvoiceItems = allUnsubmittedItems.Where(x => submitInvoiceNos.Contains(x.InvoiceNumber));

            var invoicesWhereNotAllJobsIncluded = allInvoiceItems.Where(x => !submitJobIds.Contains(x.JobId)).ToArray();
            if (invoicesWhereNotAllJobsIncluded.Any())
            {
                var invoiceNos = string.Join(",", invoicesWhereNotAllJobsIncluded.Select(x => x.InvoiceNumber).Distinct());
                return new SubmitActionResult { IsValid = false, Message = $"Not all jobs have been submitted for credit for invoice nos '{invoiceNos}'" };
            }

            return new SubmitActionResult { IsValid = true };
        }

        public SubmitActionResult ValidateUserForCrediting()
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