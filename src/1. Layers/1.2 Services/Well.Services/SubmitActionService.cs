namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Common.Contracts;
    using Contracts;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class SubmitActionService : ISubmitActionService
    {
        private readonly ILogger logger;
        private readonly IUserNameProvider userNameProvider;
        private readonly ILineItemActionRepository lineItemActionRepository;
        private readonly IDeliveryLineCreditMapper deliveryLineCreditMapper;
        private readonly ICreditTransactionFactory creditTransactionFactory;
        private readonly IExceptionEventRepository exceptionEventRepository;
        private readonly ISubmitActionValidation validator;
        private readonly IUserThresholdService userThresholdService;
        private readonly IActionSummaryMapper actionSummaryMapper;

        public SubmitActionService(
            ILogger logger,
            IUserNameProvider userNameProvider,
            ILineItemActionRepository lineItemActionRepository,
            IDeliveryLineCreditMapper deliveryLineCreditMapper,
            ICreditTransactionFactory creditTransactionFactory,
            IExceptionEventRepository exceptionEventRepository,
            ISubmitActionValidation validator,
            IUserThresholdService userThresholdService,
            IActionSummaryMapper actionSummaryMapper)
        {
            this.logger = logger;
            this.userNameProvider = userNameProvider;
            this.lineItemActionRepository = lineItemActionRepository;
            this.deliveryLineCreditMapper = deliveryLineCreditMapper;
            this.creditTransactionFactory = creditTransactionFactory;
            this.exceptionEventRepository = exceptionEventRepository;
            this.validator = validator;
            this.userThresholdService = userThresholdService;
            this.actionSummaryMapper = actionSummaryMapper;
        }

        public SubmitActionResult SubmitAction(SubmitActionModel submitAction)
        {
            SubmitActionResult result = null;
            var unsubmittedItems = lineItemActionRepository.GetUnsubmittedActions(submitAction.Action).ToArray();

            submitAction.SetItemsToSubmit(unsubmittedItems);

            result = validator.Validate(submitAction, unsubmittedItems);

            if (!result.IsValid)
            {
                return result;
            }

            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    if (submitAction.Action == DeliveryAction.Credit)
                    {
                        foreach (var jobId in submitAction.ItemsToSubmit.Select(x => x.JobId).Distinct())
                        {
                            var jobItems = submitAction.ItemsToSubmit.Where(x => x.JobId == jobId).ToArray();
                            if (UserHasRequiredCreditThreshold(jobId, submitAction.ItemsToSubmit))
                            {
                                CreditJobInAdam(jobItems);
                                SaveItemsAsSubmittedAndApproved(jobItems);
                            }
                            else
                            {
                                SaveItemsAsSubmitted(jobItems);
                                var invoiceNo = submitAction.ItemsToSubmit.First(x => x.JobId == jobId).InvoiceNumber;
                                if (!result.Warnings.Any(x => x.Contains(invoiceNo)))
                                {
                                    result.Warnings.Add($"Your threshold level is not high enough to credit delivery for invoice no: {invoiceNo}. " +
                                                        $"It has been marked for authorisation.");
                                }
                            }
                        }
                    }
                    else
                    {
                        SaveItemsAsSubmitted(submitAction.ItemsToSubmit);
                    }

                    transactionScope.Complete();
                }
                result.IsValid = true;
                result.Message = $"Submitted  {submitAction.Action} successfully for JobIds {string.Join(",", submitAction.JobIds)}";
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error Submitting Action {submitAction.Action} for JobIds {string.Join(",", submitAction.JobIds)}", ex);
                return new SubmitActionResult { Message = $"Error submitting  {submitAction.Action}. No actions have been processed" };
            }
        }

        public ActionSubmitSummary GetSubmitSummary(SubmitActionModel submitAction, bool isStopLevel)
        {
            SubmitActionResult result = null;
            var unsubmittedItems = lineItemActionRepository.GetUnsubmittedActions(submitAction.Action).ToArray();
            submitAction.SetItemsToSubmit(unsubmittedItems);

            result = validator.Validate(submitAction, unsubmittedItems);
            if (!result.IsValid)
            {
                return new ActionSubmitSummary { Summary = result.Message };
            }

            return actionSummaryMapper.Map(submitAction, isStopLevel);
        }

        private bool UserHasRequiredCreditThreshold(int jobId, IEnumerable<LineItemActionSubmitModel> submitActionItemsToSubmit)
        {
            submitActionItemsToSubmit = submitActionItemsToSubmit.ToArray();
            var invoiceNo = submitActionItemsToSubmit.First(x => x.JobId == jobId).InvoiceNumber;
            var creditValue = submitActionItemsToSubmit.Where(x => x.InvoiceNumber == invoiceNo).Sum(x => x.TotalValue);
            return userThresholdService.CanUserCredit(creditValue).CanUserCredit;
        }

        private void SaveItemsAsSubmitted(IEnumerable<LineItemActionSubmitModel> items)
        {
            foreach (var lineItemAction in items)
            {
                lineItemAction.SubmittedDate = DateTime.Now;
                lineItemAction.ActionedBy = userNameProvider.GetUserName();
                lineItemActionRepository.Update(lineItemAction);
            }
        }

        private void SaveItemsAsSubmittedAndApproved(IEnumerable<LineItemActionSubmitModel> items)
        {
            foreach (var lineItemAction in items)
            {
                lineItemAction.SubmittedDate = DateTime.Now;
                lineItemAction.ActionedBy = userNameProvider.GetUserName();
                lineItemAction.ApprovalDate = DateTime.Now;
                lineItemAction.ApprovedBy = userNameProvider.GetUserName();
                lineItemActionRepository.Update(lineItemAction);
            }
        }

        private void CreditJobInAdam(LineItemActionSubmitModel[] jobItemsToCredit)
        {
            if (jobItemsToCredit == null || !jobItemsToCredit.Any())
            {
                throw new ArgumentNullException(nameof(jobItemsToCredit));
            }
            var branchId = jobItemsToCredit.First().BranchId;
            var credits = deliveryLineCreditMapper.Map(jobItemsToCredit);
            var creditEventTransaction = this.creditTransactionFactory.Build(credits, branchId);
            exceptionEventRepository.InsertCreditEventTransaction(creditEventTransaction);
        }
    }
}