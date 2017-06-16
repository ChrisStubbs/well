namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Common.Contracts;
    using Contracts;
    using Domain;
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
        private readonly IJobRepository jobRepository;
        private readonly ILineItemSearchReadRepository lineItemRepository;

        public SubmitActionService(
            ILogger logger,
            IUserNameProvider userNameProvider,
            ILineItemActionRepository lineItemActionRepository,
            IDeliveryLineCreditMapper deliveryLineCreditMapper,
            ICreditTransactionFactory creditTransactionFactory,
            IExceptionEventRepository exceptionEventRepository,
            ISubmitActionValidation validator,
            IUserThresholdService userThresholdService,
            IActionSummaryMapper actionSummaryMapper,
            IJobRepository jobRepository,
            ILineItemSearchReadRepository lineItemRepository)
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
            this.jobRepository = jobRepository;
            this.lineItemRepository = lineItemRepository;
        }

        public SubmitActionResult SubmitAction(SubmitActionModel submitAction)
        {
            SubmitActionResult result = null;

            var jobs = jobRepository.GetByIds(submitAction.JobIds).ToList();
            var lineItems = lineItemRepository.GetLineItemByJobIds(submitAction.JobIds);
            var jobRoutes = jobRepository.GetJobsRoute(jobs.Select(x => x.Id));

            jobs.ForEach(job =>
                {
                    job.LineItems = lineItems.Where(x => x.JobId == job.Id).ToList();
                    job.JobRoute = jobRoutes.Single(x => x.JobId == job.Id);
                }
            );


            result = validator.Validate(submitAction, jobs);

            if (!result.IsValid)
            {
                return result;
            }


            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    foreach (var job in jobs)
                    {
                        // after validation will only have pending submission Jobs
                        if (job.ResolutionStatus == ResolutionStatus.PendingSubmission)
                        {

                            var jobLineItemActions = job.GetAllLineItemActions();
                            if (jobLineItemActions.Any(x => x.DeliveryAction == DeliveryAction.Credit))
                            {
                                //if (userThresholdService.UserHasRequiredCreditThreshold(job))
                                //{
                                    CreditJobInAdam(job);
                                //}
                            }
                            else
                            {
                                
                            }

                            //TODD: Call Henrris Terrific  code

                        }
                    }

                    //if (submitAction.Action == DeliveryAction.Credit)
                    //{
                    //    foreach (var jobId in submitAction.ItemsToSubmit.Select(x => x.JobId).Distinct())
                    //    {
                    //        var jobItems = submitAction.ItemsToSubmit.Where(x => x.JobId == jobId).ToArray();
                    //        if (UserHasRequiredCreditThreshold(jobId, submitAction.ItemsToSubmit))
                    //        {
                    //            CreditJobInAdam(jobItems);
                    //            SaveItemsAsSubmittedAndApproved(jobItems);
                    //        }
                    //        else
                    //        {
                    //            SaveItemsAsSubmitted(jobItems);
                    //            var invoiceNo = submitAction.ItemsToSubmit.First(x => x.JobId == jobId).InvoiceNumber;
                    //            if (!result.Warnings.Any(x => x.Contains(invoiceNo)))
                    //            {
                    //                result.Warnings.Add($"Your threshold level is not high enough to credit delivery for invoice no: {invoiceNo}. " +
                    //                                    $"It has been marked for authorisation.");
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    SaveItemsAsSubmitted(submitAction.ItemsToSubmit);
                    //}

                    transactionScope.Complete();
                }
                result.IsValid = true;
                result.Message = $"Submitted  actions successfully for JobIds {string.Join(",", submitAction.JobIds)}";
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error Submitting actions for JobIds {string.Join(",", submitAction.JobIds)}", ex);
                return new SubmitActionResult { Message = "Error submitting. No actions have been processed" };
            }
        }
        private void CreditJobInAdam(Job job)
        {
            var credits = deliveryLineCreditMapper.Map(job);
            var creditEventTransaction = this.creditTransactionFactory.Build(credits, job.JobRoute.BranchId);
            exceptionEventRepository.InsertCreditEventTransaction(creditEventTransaction);
        }

        public ActionSubmitSummary GetSubmitSummary(SubmitActionModel submitAction, bool isStopLevel)
        {
            throw new NotImplementedException("Do This");
        //    SubmitActionResult result = null;
        //    var unsubmittedItems = lineItemActionRepository.GetUnsubmittedActions(submitAction.Action).ToArray();
        //    submitAction.SetItemsToSubmit(unsubmittedItems);

        //    result = validator.Validate(submitAction, unsubmittedItems);
        //    if (!result.IsValid)
        //    {
        //        return new ActionSubmitSummary { Summary = result.Message };
        //    }

        //    return actionSummaryMapper.Map(submitAction, isStopLevel);
        }



        //private void SaveItemsAsSubmitted(IEnumerable<LineItemActionSubmitModel> items)
        //{
        //    foreach (var lineItemAction in items)
        //    {
        //        lineItemAction.SubmittedDate = DateTime.Now;
        //        lineItemAction.ActionedBy = userNameProvider.GetUserName();
        //        lineItemActionRepository.Update(lineItemAction);
        //    }
        //}

        //private void SaveItemsAsSubmittedAndApproved(IEnumerable<LineItemActionSubmitModel> items)
        //{
        //    foreach (var lineItemAction in items)
        //    {
        //        lineItemAction.SubmittedDate = DateTime.Now;
        //        lineItemAction.ActionedBy = userNameProvider.GetUserName();
        //        lineItemAction.ApprovalDate = DateTime.Now;
        //        lineItemAction.ApprovedBy = userNameProvider.GetUserName();
        //        lineItemActionRepository.Update(lineItemAction);
        //    }
        //}

        
    }
}