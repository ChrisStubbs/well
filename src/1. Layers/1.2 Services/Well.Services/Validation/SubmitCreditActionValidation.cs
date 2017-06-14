namespace PH.Well.Services.Validation
{
    using System;
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
        private readonly IDateThresholdService dateThresholdService;

        public SubmitCreditActionValidation(
            IUserNameProvider userNameProvider,
            IUserRepository userRepository,
            IDateThresholdService dateThresholdService)
        {
            this.userNameProvider = userNameProvider;
            this.userRepository = userRepository;
            this.dateThresholdService = dateThresholdService;
        }

        public SubmitActionResult Validate(SubmitActionModel submitAction ,IEnumerable<LineItemActionSubmitModel> allUnsubmittedItems)
        {
            allUnsubmittedItems = allUnsubmittedItems.ToArray();
            var result = new SubmitActionResult { IsValid = true };

            if (submitAction.Action != DeliveryAction.Credit)
            {
                return result;
            }

            result = ValidateUserForCrediting();

            if (result.IsValid)
            {
                result = AllCreditItemsForInvoiceIncluded(submitAction, allUnsubmittedItems);
                if (result.IsValid)
                {
                    result = EarliestCreditDateForItemsHasBeenReached(submitAction, allUnsubmittedItems);
                }
            }

            return result;
        }

        public virtual SubmitActionResult EarliestCreditDateForItemsHasBeenReached(SubmitActionModel submitAction, IEnumerable<LineItemActionSubmitModel> allUnsubmittedItems)
        {
            allUnsubmittedItems = allUnsubmittedItems.ToArray();
            submitAction.SetItemsToSubmit(allUnsubmittedItems);
            var itemsBeforeEarliestCreditDate = submitAction.ItemsToSubmit.Where(x => DateTime.Now < dateThresholdService.EarliestCreditDate(x.RouteDate, x.BranchId)).ToArray();

            if (itemsBeforeEarliestCreditDate.Any())
            {
                var invoiceNos = string.Join(",", itemsBeforeEarliestCreditDate.Select(
                                                        x => $"{x.InvoiceNumber}: earliest credit date: {dateThresholdService.EarliestCreditDate(x.RouteDate, x.BranchId)}"
                                                        ).Distinct());

                return new SubmitActionResult { IsValid = false, Message = $"Invoice nos: '{invoiceNos}' have not reached the earliest credit date so can not be submitted." };
            }

            return new SubmitActionResult { IsValid = true };
        }


        public virtual SubmitActionResult AllCreditItemsForInvoiceIncluded(SubmitActionModel submitAction, IEnumerable<LineItemActionSubmitModel> allUnsubmittedItems)
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