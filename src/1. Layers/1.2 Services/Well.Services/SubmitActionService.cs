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
        private readonly IDeliveryLineCreditMapper deliveryLineCreditMapper;
        private readonly ICreditTransactionFactory creditTransactionFactory;
        private readonly IExceptionEventRepository exceptionEventRepository;
        private readonly ISubmitActionValidation validator;
        private readonly IActionSummaryMapper actionSummaryMapper;
        private readonly IJobRepository jobRepository;
        private readonly IJobResolutionStatus jobResolutionStatus;
        private readonly IJobService jobService;
        private readonly IUserRepository userRepository;

        public SubmitActionService(
            ILogger logger,
            IDeliveryLineCreditMapper deliveryLineCreditMapper,
            ICreditTransactionFactory creditTransactionFactory,
            IExceptionEventRepository exceptionEventRepository,
            ISubmitActionValidation validator,
            IActionSummaryMapper actionSummaryMapper,
            IJobRepository jobRepository,
            IJobResolutionStatus jobResolutionStatus,
            IJobService jobService,
            IUserRepository userRepository
            )
        {
            this.logger = logger;
            this.deliveryLineCreditMapper = deliveryLineCreditMapper;
            this.creditTransactionFactory = creditTransactionFactory;
            this.exceptionEventRepository = exceptionEventRepository;
            this.validator = validator;
            this.actionSummaryMapper = actionSummaryMapper;
            this.jobRepository = jobRepository;
            this.jobResolutionStatus = jobResolutionStatus;
            this.jobService = jobService;
            this.userRepository = userRepository;
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

            var resultDetails = new List<SubmitActionResultDetails>();
            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    foreach (var job in jobs)
                    {
                        // after validation will only have pending submission Jobs
                        if (jobResolutionStatus.GetCurrentResolutionStatus(job) == ResolutionStatus.PendingSubmission
                            || jobResolutionStatus.GetCurrentResolutionStatus(job) == ResolutionStatus.PendingApproval)
                        {
                            job.ResolutionStatus = jobResolutionStatus.GetNextResolutionStatus(job);
                            jobRepository.SaveJobResolutionStatus(job);

                            if (jobResolutionStatus.GetCurrentResolutionStatus(job) == ResolutionStatus.Approved)
                            {
                                SubmitCredits(job);
                                job.ResolutionStatus = jobResolutionStatus.GetNextResolutionStatus(job);
                                jobRepository.SaveJobResolutionStatus(job);
                            }
                            else
                            {
                                //the job is in ResolutionStatus.PendingApproval
                                //so it have to be unallocated 
                                this.userRepository.UnAssignJobToUser(job.Id);
                                result.Warnings.Add("Your threshold level is not high enough " +
                                                    $"to credit the delivery for job no: {job.Id} invoice: {job.InvoiceNumber}. " +
                                                    "The Job has been been marked for authorisation.");
                            }

                            
                            jobRepository.Update(job);
                            resultDetails.Add(new SubmitActionResultDetails(job));
                        }
                        
                    }

                    transactionScope.Complete();
                }

                result.IsValid = true;
                result.Message = "Successfully Submitted Actions";
                result.Details = resultDetails;
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
            jobs = jobService.PopulateLineItemsAndRoute(jobs).ToList();
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