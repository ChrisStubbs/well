namespace PH.Well.Services.DeliveryActions
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class DeliveryLinesCredit : BaseDeliveryAction, IDeliveryLinesAction
    {
        private readonly IUserThresholdService userThresholdService;
        private readonly IJobDetailToDeliveryLineCreditMapper mapper;
        private readonly ICreditTransactionFactory creditTransactionFactory;
        private readonly IAdamRepository adamRepository;
        private readonly IExceptionEventRepository eventRepository;
        private readonly IBranchRepository branchRepository;
        private readonly IDeliveryReadRepository deliveryReadRepository;

        public DeliveryLinesCredit(
            IUserThresholdService userThresholdService,
            IJobDetailToDeliveryLineCreditMapper mapper,
            ICreditTransactionFactory creditTransactionFactory,
            IAdamRepository adamRepository,
            IExceptionEventRepository eventRepository,
            IBranchRepository branchRepository)
        {
            this.userThresholdService = userThresholdService;
            this.mapper = mapper;
            this.creditTransactionFactory = creditTransactionFactory;
            this.adamRepository = adamRepository;
            this.eventRepository = eventRepository;
            this.branchRepository = branchRepository;
        }

        public DeliveryAction Action => DeliveryAction.Credit;

        public ProcessDeliveryActionResult Execute(Job job)
        {
            var lines = GetJobDetailsByAction(job, Action);

            if (lines.Any() == false)
            {
                return new ProcessDeliveryActionResult();
            }

            return CreditDeliveryLines(job.Id, lines);
        }

        private ProcessDeliveryActionResult CreditDeliveryLines(int jobId, IEnumerable<JobDetail> creditLines)
        {
            var result = new ProcessDeliveryActionResult();

            var branchId = this.branchRepository.GetBranchIdForJob(jobId);
            AdamSettings adamSettings = AdamSettingsFactory.GetAdamSettings((Domain.Enums.Branch) branchId);

            var totalThresholdValue = creditLines.Sum(x => x.CreditValueForThreshold());

            // is the user allowed to credit this amount or does it need to go to the next threshold user
            var thresholdResponse = this.userThresholdService.CanUserCredit(totalThresholdValue);

            if (thresholdResponse.IsInError)
            {
                result.Warnings.Add(thresholdResponse.ErrorMessage);
            }
            else
            {
                if (!thresholdResponse.CanUserCredit)
                {
                    result.Warnings.Add(
                        "Your threshold level is not high enough to credit this order. It has been passed on for authorisation.");
                    this.userThresholdService.AssignPendingCredit(branchId, totalThresholdValue,
                        creditLines.First().JobId);
                }
                else
                {
                    result.AdamIsDown = this.Credit(creditLines, adamSettings, branchId) != AdamResponse.Success;
                }
            }


            return result;
        }

        private AdamResponse Credit(IEnumerable<JobDetail> creditLines, AdamSettings adamSettings, int branchId)
        {
            var jobId = creditLines.First().JobId;

            var credits = this.mapper.Map(creditLines);

            var creditEventTransaction = this.creditTransactionFactory.Build(credits, branchId);

            var response = this.adamRepository.Credit(creditEventTransaction, adamSettings);
            
            foreach (var creditLine in creditLines)
            {
                if (creditLine.ShortsAction == DeliveryAction.Credit)
                {
                    creditLine.ShortsStatus = JobDetailStatus.Res;
                }
                foreach (var jobDetailDamage in creditLine.JobDetailDamages)
                {
                    if (jobDetailDamage.DamageAction == DeliveryAction.Credit)
                    {
                        jobDetailDamage.DamageStatus = JobDetailStatus.Res;
                    }
                }
            }

            if (response == AdamResponse.AdamDown || response == AdamResponse.Unknown)
            {
                this.eventRepository.InsertCreditEventTransaction(creditEventTransaction);
                return response;
            }

            this.eventRepository.RemovedPendingCredit(jobId);

            return AdamResponse.Success;
        }
    }
}
