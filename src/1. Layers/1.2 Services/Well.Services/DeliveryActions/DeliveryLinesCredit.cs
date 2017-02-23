namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain.Enums;
    using Domain.ValueObjects;
    using Repositories.Contracts;

    public class DeliveryLinesCredit : IDeliveryLinesAction
    {
        private readonly IUserThresholdService userThresholdService;
        private readonly IJobRepository jobRepository;
        private readonly IDeliverLineToDeliveryLineCreditMapper mapper;
        private readonly ICreditTransactionFactory creditTransactionFactory;
        private readonly IAdamRepository adamRepository;
        private readonly IExceptionEventRepository eventRepository;

        public DeliveryLinesCredit(
            IUserThresholdService userThresholdService,
            IJobRepository jobRepository,
            IDeliverLineToDeliveryLineCreditMapper mapper,
            ICreditTransactionFactory creditTransactionFactory,
            IAdamRepository adamRepository,
            IExceptionEventRepository eventRepository)
        {
            this.userThresholdService = userThresholdService;
            this.jobRepository = jobRepository;
            this.mapper = mapper;
            this.creditTransactionFactory = creditTransactionFactory;
            this.adamRepository = adamRepository;
            this.eventRepository = eventRepository;
        }

        public DeliveryAction Action
        {
            get
            {
                return DeliveryAction.Credit;
            }
        }

        public ProcessDeliveryActionResult Execute(Func<DeliveryAction, IList<DeliveryLine>> deliveryLines, AdamSettings adamSettings, string username, int branchId)
        {
            var lines = deliveryLines(this.Action);

            return CreditDeliveryLines(lines, adamSettings, username, branchId);
        }

        private ProcessDeliveryActionResult CreditDeliveryLines(IList<DeliveryLine> creditLines, AdamSettings adamSettings, string username, int branchId)
        {
            var result = new ProcessDeliveryActionResult();

            if (creditLines.Any())
            {
                var totalThresholdValue = creditLines.Sum(x => x.CreditValueForThreshold());

                // is the user allowed to credit this amount or does it need to go to the next threshold user
                var thresholdResponse = this.userThresholdService.CanUserCredit(username, totalThresholdValue);

                if (thresholdResponse.IsInError)
                {
                    result.Warnings.Add(thresholdResponse.ErrorMessage);
                }
                else
                {
                    if (!thresholdResponse.CanUserCredit)
                    {
                        result.Warnings.Add("Your threshold level isn\'t high enough for the credit... It has been passed on for authorisation...");
                        this.userThresholdService.AssignPendingCredit(branchId, totalThresholdValue, creditLines[0].JobId, username);
                    }
                    else
                    {
                        result.AdamIsDown = this.Credit(creditLines, adamSettings, username, branchId) != AdamResponse.Success;
                    }
                }
            }

            return result;
        }

        private AdamResponse Credit(IList<DeliveryLine> creditLines, AdamSettings adamSettings, string username, int branchId)
        {
            var job = this.jobRepository.GetById(creditLines[0].JobId);

            var credits = this.mapper.Map(creditLines);

            var creditEventTransaction = this.creditTransactionFactory.Build(credits, username, branchId);

            var response = this.adamRepository.Credit(creditEventTransaction, adamSettings, username);

            if (response == AdamResponse.AdamDown || response == AdamResponse.Unknown)
            {
                this.eventRepository.CurrentUser = username;
                this.eventRepository.InsertCreditEventTransaction(creditEventTransaction);
                // update job here to show that the job has been actioned
                this.jobRepository.SetJobToSubmittedStatus(job.Id);

                return response;
            }

            this.eventRepository.RemovedPendingCredit(job.Id);

            return AdamResponse.Success;
        }
    }
}
