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
        private readonly IUserRepository userRepository;
        private readonly ILineItemActionRepository lineItemActionRepository;
        private readonly IDeliveryLineCreditMapper deliveryLineCreditMapper;
        private readonly ICreditTransactionFactory creditTransactionFactory;
        private readonly IExceptionEventRepository exceptionEventRepository;

        public SubmitActionService(
            ILogger logger,
            IUserNameProvider userNameProvider,
            IUserRepository userRepository,
            ILineItemActionRepository lineItemRepository,
            IDeliveryLineCreditMapper deliveryLineCreditMapper,
            ICreditTransactionFactory creditTransactionFactory,
            IExceptionEventRepository exceptionEventRepository)
        {
            this.logger = logger;
            this.userNameProvider = userNameProvider;
            this.userRepository = userRepository;
            this.lineItemActionRepository = lineItemRepository;
            this.deliveryLineCreditMapper = deliveryLineCreditMapper;
            this.creditTransactionFactory = creditTransactionFactory;
            this.exceptionEventRepository = exceptionEventRepository;
        }

        public SubmitActionResult SubmitAction(SubmitActionModel submitAction)
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
                return new SubmitActionResult { Message = "User not assigned to all the job submitted can not submit action" };
            }

            LineItemActionSubmitModel[] itemsToSubmit = lineItemActionRepository.GetLineItemsWithUnsubmittedActions(submitAction.JobIds, submitAction.Action).ToArray();

            if (!itemsToSubmit.Any())
            {
                return new SubmitActionResult { Message = $"There are no {submitAction.Action} Actions for the jobs submitted" };
            }

            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    if (submitAction.Action == DeliveryAction.Credit)
                    {
                        foreach (var jobId in itemsToSubmit.Select(x => x.JobId).Distinct())
                        {
                            var jobItems = itemsToSubmit.Where(x => x.JobId == jobId).ToArray();
                            CreditJobInAdam(jobItems);
                            SaveItemsAsSubmitted(jobItems);
                        }
                    }
                    else
                    {
                        SaveItemsAsSubmitted(itemsToSubmit);
                    }
                    transactionScope.Complete();
                }
                
                return new SubmitActionResult { IsValid = true, Message = $"Submitted  {submitAction.Action} successfully for JobIds {string.Join(",", submitAction.JobIds)}" };
            }
            catch (Exception ex)
            {
                logger.LogError($"Error Submitting Action {submitAction.Action} for JobIds {string.Join(",", submitAction.JobIds)}", ex);
                return new SubmitActionResult { Message = $"Error submitting  {submitAction.Action}. No actions have been processed" };
            }
        }

        private void SaveItemsAsSubmitted(IEnumerable<LineItemActionSubmitModel> items)
        {
            foreach (var lineItemAction in items)
            {
                lineItemAction.SubmittedDate = DateTime.Now;
                lineItemActionRepository.Save(lineItemAction);
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