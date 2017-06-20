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
        private readonly IJobResolutionStatus jobResolutionStatus;

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
            ILineItemSearchReadRepository lineItemRepository,
            IJobResolutionStatus jobResolutionStatus
            )
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
            this.jobResolutionStatus = jobResolutionStatus;
        }

        public SubmitActionResult SubmitAction(SubmitActionModel submitAction)
        {
            SubmitActionResult result = null;

            List<Job> jobs = GetJobs(submitAction);

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
                        if (jobResolutionStatus.GetStatus(job) == ResolutionStatus.PendingSubmission)
                        {
                            job.ResolutionStatus = jobResolutionStatus.StepForward(job);
                            jobRepository.SaveJobResolutionStatus(job);

                            if (jobResolutionStatus.GetStatus(job) == ResolutionStatus.Approved)
                            {
                                SubmitCredits(job);
                                job.ResolutionStatus = jobResolutionStatus.StepForward(job);
                                jobRepository.SaveJobResolutionStatus(job);
                            }
                            else
                            {
                                result.Warnings.Add("Your threshold level is not high enough " +
                                                    $"to credit the delivery for job no: {job.Id} invoice: {job.InvoiceNumber}. " +
                                                    "The Job has been been marked for authorisation.");
                            }

                           
                            jobRepository.Update(job);
                        }
                    }

                    transactionScope.Complete();
                }

                result.IsValid = true;
                result.Message = "Successfully Submitted Actions";
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError($"Error Submitting actions for JobIds {string.Join(",", submitAction.JobIds)}", ex);
                return new SubmitActionResult { Message = "Error submitting. No actions have been processed" };
            }
        }

        private List<Job> GetJobs(SubmitActionModel submitAction)
        {
            var jobs = jobRepository.GetByIds(submitAction.JobIds).ToList();
            var lineItems = lineItemRepository.GetLineItemByJobIds(submitAction.JobIds);
            var jobRoutes = jobRepository.GetJobsRoute(jobs.Select(x => x.Id));

            jobs.ForEach(job =>
                {
                    job.LineItems = lineItems.Where(x => x.JobId == job.Id).ToList();
                    job.JobRoute = jobRoutes.Single(x => x.JobId == job.Id);
                }
            );

            return jobs;
        }

        private void SubmitCredits(Job job)
        {
            var jobLineItemActions = job.GetAllLineItemActions();
            if (jobLineItemActions.Any(x => x.DeliveryAction == DeliveryAction.Credit))
            {
                CreditJobInAdam(job);
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
            SubmitActionResult result = null;
            List<Job> jobs = GetJobs(submitAction);

            result = validator.Validate(submitAction, jobs);
            if (!result.IsValid)
            {
                return new ActionSubmitSummary { Summary = result.Message };
            }

            return actionSummaryMapper.Map(submitAction, isStopLevel, jobs);
        }

    }
}