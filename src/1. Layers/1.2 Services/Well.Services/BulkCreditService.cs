namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Repositories.Contracts;

    public class BulkCreditService : IBulkCreditService
    {
        private readonly IUserThresholdService userThresholdService;
        private readonly IBranchRepository branchRepository;
        private readonly ICreditTransactionFactory creditTransactionFactory;
        private readonly IJobDetailToDeliveryLineCreditMapper jobDetailToDeliveryLineCreditMapper;
        private readonly IExceptionEventRepository exceptionEventRepository;
        private readonly IJobRepository jobRepository;
        private readonly IUserNameProvider userNameProvider;
        private readonly IUserRepository userRepository;
        private readonly IJobDetailRepository jobDetailRepository;

        public BulkCreditService(IUserThresholdService userThresholdService,
            IBranchRepository branchRepository,
            ICreditTransactionFactory creditTransactionFactory,
            IJobDetailToDeliveryLineCreditMapper jobDetailToDeliveryLineCreditMapper,
            IExceptionEventRepository exceptionEventRepository,
            IJobRepository jobRepository,
            IUserNameProvider userNameProvider,
            IUserRepository userRepository,
            IJobDetailRepository jobDetailRepository)
        {
            this.userThresholdService = userThresholdService;
            this.branchRepository = branchRepository;
            this.creditTransactionFactory = creditTransactionFactory;
            this.jobDetailToDeliveryLineCreditMapper = jobDetailToDeliveryLineCreditMapper;
            this.exceptionEventRepository = exceptionEventRepository;
            this.jobRepository = jobRepository;
            this.userNameProvider = userNameProvider;
            this.userRepository = userRepository;
            this.jobDetailRepository = jobDetailRepository;
        }

        public IEnumerable<string> BulkCredit(IEnumerable<Job> jobs, JobDetailReason reason, JobDetailSource source)
        {
            var warning = ValidateUserForCrediting();
            if (string.IsNullOrWhiteSpace(warning) == false)
            {
                return new List<string>() { warning };
            }

            var jobsList = jobs.ToList();
            SetCreditActions(jobsList, reason, source);

            var warnings = new List<string>();
            using (var transactionScope = new TransactionScope())
            {
                foreach (var job in jobsList)
                {
                    if (CreditJob(job, warnings))
                    {
                        jobRepository.Update(job);
                        foreach (var jobDetail in job.JobDetails)
                        {
                            jobDetailRepository.Update(jobDetail);
                        }
                    }
                }
                transactionScope.Complete();
            }

            return warnings;
        }

        public void SetCreditActions(List<Job> jobs, JobDetailReason reason, JobDetailSource source)
        {
            foreach (var job in jobs)
            {
                foreach (var jobDetail in job.JobDetails)
                {
                    if (jobDetail.ShortQty > 0)
                    {
                        jobDetail.ShortsActionId = (int)DeliveryAction.Credit;
                        jobDetail.JobDetailReasonId = (int)reason;
                        jobDetail.JobDetailSourceId = (int)source;
                    }
                    foreach (var damage in jobDetail.JobDetailDamages)
                    {
                        if (damage.Qty > 0)
                        {
                            damage.DamageActionId = (int)DeliveryAction.Credit;
                            damage.JobDetailReasonId = (int)reason;
                            damage.JobDetailSourceId = (int)source;
                        }
                    }
                }
            }
        }

        public bool CreditJob(Job job, List<string> warnings)
        {
            IEnumerable<JobDetail> jobDetailsToCredit = job.JobDetails.Where(l => l.ShortsAction == DeliveryAction.Credit ||
                                                               l.JobDetailDamages.Any(d => d.DamageAction == DeliveryAction.Credit));

            if (!job.HasDamages && !job.HasShorts)
            {
                warnings.Add($"Invoice no: {job.InvoiceNumber} has no Damages or Shorts");
                return false;
            }

            var totalThresholdValue = jobDetailsToCredit.Sum(x => x.CreditValueForThreshold());

            // is the user allowed to credit this amount or does it need to go to the next threshold user
            var thresholdResponse = this.userThresholdService.CanUserCredit(totalThresholdValue);


            if (thresholdResponse.IsInError)
            {
                warnings.Add(thresholdResponse.ErrorMessage);
                return false;
            }

            var branchId = this.branchRepository.GetBranchIdForJob(job.Id);
            if (!thresholdResponse.CanUserCredit)
            {
                warnings.Add($"Your threshold level is not high enough to credit delivery for invoice no: {job.InvoiceNumber}. " +
                    $"It has been passed on for authorisation.");
                string assignedWarning = this.userThresholdService.AssignPendingCredit(branchId, totalThresholdValue, job.Id);
                if (string.IsNullOrEmpty(assignedWarning) == false)
                {
                    warnings.Add(assignedWarning);
                }
                return false;
            }


            var credits = jobDetailToDeliveryLineCreditMapper.Map(jobDetailsToCredit);
            var creditEventTransaction = this.creditTransactionFactory.Build(credits, branchId);
            exceptionEventRepository.InsertCreditEventTransaction(creditEventTransaction);
            ResolveJob(job);
            return true;


        }

        public void ResolveJob(Job job)
        {
            foreach (var jobDetail in job.JobDetails)
            {
                if (jobDetail.ShortQty > 0)
                {
                    jobDetail.ShortsStatus = JobDetailStatus.Res;

                }
                foreach (var damage in jobDetail.JobDetailDamages)
                {
                    if (damage.Qty > 0)
                    {
                        damage.DamageStatus = JobDetailStatus.Res;
                    }
                }
            }

            if (job.CanResolve)
            {
                job.JobStatus = JobStatus.Resolved;
            }
        }

        private string ValidateUserForCrediting()
        {
            var username = this.userNameProvider.GetUserName();
            var user = this.userRepository.GetByIdentity(username);

            if (user == null)
            {
                return $"User not found ({username})";
            }

            if (user.ThresholdLevelId == null)
            {
                return $"You must be assigned a threshold level before crediting.";
            }

            return string.Empty;
        }


    }
}
