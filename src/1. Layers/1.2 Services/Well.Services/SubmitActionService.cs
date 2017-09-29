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
        private readonly IJobService jobService;
        private readonly IUserRepository userRepository;
        private readonly IPodService podService;

        public SubmitActionService(
            ILogger logger,
            IDeliveryLineCreditMapper deliveryLineCreditMapper,
            ICreditTransactionFactory creditTransactionFactory,
            IExceptionEventRepository exceptionEventRepository,
            ISubmitActionValidation validator,
            IActionSummaryMapper actionSummaryMapper,
            IJobRepository jobRepository,
            IJobService jobService,
            IUserRepository userRepository,
            IPodService podService
            )
        {
            this.logger = logger;
            this.deliveryLineCreditMapper = deliveryLineCreditMapper;
            this.creditTransactionFactory = creditTransactionFactory;
            this.exceptionEventRepository = exceptionEventRepository;
            this.validator = validator;
            this.actionSummaryMapper = actionSummaryMapper;
            this.jobRepository = jobRepository;
            this.jobService = jobService;
            this.userRepository = userRepository;
            this.podService = podService;
        }
        private SubmitActionResult Reject(int[] jobsId)
        {
            var result = new SubmitActionResult();

            foreach (var job in GetJobs(jobsId))
            {
                var status = jobService.StepBack(job);

                if (status == ResolutionStatus.Invalid)
                {
                    result.Warnings.Add($"Job no {job.Id} invoice: {job.InvoiceNumber} can not be rejected.");
                }
                else
                {
                    job.ResolutionStatus = status;
                    jobRepository.SaveJobResolutionStatus(job);
                    jobRepository.Update(job);
                }
            }

            if (result.Warnings.Count == jobsId.Count())
            {
                result.Message = "No jobs were rejected.";
                result.IsValid = false;
            }
            else if (result.Warnings.Any())
            {
                result.Message = "One or more jobs could not be submitted";
                result.IsValid = true;
            }
            else
            {
                result.Message = "All jobs were successfully rejected.";
                result.IsValid = true;
            }

            return result;
        }

        private SubmitActionResult Approve(int[] jobsId)
        {
            SubmitActionResult result = null;

            List<Job> jobs = GetJobs(jobsId);

            result = validator.Validate(jobsId, jobs);

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
                        var jobResolutionStatus = jobService.GetCurrentResolutionStatus(job);
                        if (jobResolutionStatus == ResolutionStatus.PendingSubmission
                            || jobResolutionStatus == ResolutionStatus.PendingApproval)
                        {
                            job.ResolutionStatus = jobService.StepForward(job);
                            jobRepository.SaveJobResolutionStatus(job);

                            if (jobService.GetCurrentResolutionStatus(job) == ResolutionStatus.Approved)
                            {
                                if (!job.IsProofOfDelivery)
                                {
                                    SubmitCredits(job);
                                }
                                else if (job.JobStatus == JobStatus.Bypassed)
                                {
                                    this.podService.CreatePodEvent(job, job.JobRoute.BranchId, job.JobRoute.RouteDate);
                                }

                                //lets close the job
                                job.ResolutionStatus = jobService.StepForward(job);
                                jobRepository.SaveJobResolutionStatus(job);

                                //if is one of this status it means that is ready to be closed
                                if (job.ResolutionStatus == ResolutionStatus.Credited
                                    || job.ResolutionStatus == ResolutionStatus.Resolved)
                                {
                                    job.ResolutionStatus = jobService.TryCloseJob(job);
                                    jobRepository.SaveJobResolutionStatus(job);
                                }
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
                logger.LogError($"Error Submitting actions for JobIds {string.Join(",", jobsId)}", ex);
                return new SubmitActionResult { Message = "Error submitting. No actions have been processed" };
            }
        }


        public SubmitActionResult SubmitAction(SubmitActionModel submitAction)
        {
            if (submitAction.Submit)
            {
                return this.Approve(submitAction.JobIds);
            }

            return this.Reject(submitAction.JobIds);
        }

        private List<Job> GetJobs(int[] jobsId)
        {
            var jobs = jobRepository.GetByIds(jobsId).ToList();
            jobs = jobService.PopulateLineItemsAndRoute(jobs).ToList();
            return jobs;
        }

        public virtual void SubmitCredits(Job job)
        {
            var jobLineItemActions = job.GetAllLineItemActions();
            if (jobLineItemActions.Any(x => x.DeliveryAction == DeliveryAction.Credit))
            {
                CreditJobInAdam(job);
            }
        }

        public virtual void CreditJobInAdam(Job job)
        {
            var credits = deliveryLineCreditMapper.Map(job);
            var creditEventTransaction = this.creditTransactionFactory.Build(credits, job.JobRoute.BranchId);
            exceptionEventRepository.InsertCreditEventTransaction(creditEventTransaction);
        }

        public ActionSubmitSummary GetSubmitSummary(int[] jobsId, bool isStopLevel)
        {
            SubmitActionResult result = null;
            List<Job> jobs = GetJobs(jobsId);

            result = validator.Validate(jobsId, jobs);
            if (!result.IsValid)
            {
                return new ActionSubmitSummary { Summary = result.Message };
            }

            var summary = actionSummaryMapper.Map(isStopLevel, jobs);
            summary.Warnings = result.Warnings;

            return summary;
        }
    }
}