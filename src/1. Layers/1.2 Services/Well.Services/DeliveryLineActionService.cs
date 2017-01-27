namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class DeliveryLineActionService : IDeliveryLineActionService
    {
        private readonly IAdamRepository adamRepository;
        private readonly IExceptionEventRepository eventRepository;
        private readonly IJobRepository jobRepository;
        private readonly IUserRepository userRepository;
        private readonly ICreditTransactionFactory creditTransactionFactory;
        private readonly IUserThresholdService userThresholdService;
        private readonly IDeliverLineToDeliveryLineCreditMapper mapper;

        public DeliveryLineActionService(
            IAdamRepository adamRepository,
            IExceptionEventRepository eventRepository,
            IJobRepository jobRepository,
            IUserRepository userRepository,
            ICreditTransactionFactory creditTransactionFactory,
            IUserThresholdService userThresholdService,
            IDeliverLineToDeliveryLineCreditMapper mapper)
        {
            this.adamRepository = adamRepository;
            this.eventRepository = eventRepository;
            this.jobRepository = jobRepository;
            this.userRepository = userRepository;
            this.creditTransactionFactory = creditTransactionFactory;
            this.userThresholdService = userThresholdService;
            this.mapper = mapper;
        }

        public void CreditTransaction(CreditTransaction creditTransaction, int eventId, AdamSettings adamSettings, string username)
        {
            var adamResponse = this.adamRepository.Credit(creditTransaction, adamSettings, username);

            this.MarkAsDone(eventId, adamResponse, username);
        }

        private AdamResponse Credit(IList<DeliveryLine> creditLines, AdamSettings adamSettings, string username, int branchId)
        {
            var job = this.jobRepository.GetById(creditLines[0].JobId);

            var credits = this.mapper.Map(creditLines);
            
            var creditEventTransaction = this.creditTransactionFactory.Build(credits, username, branchId);

            var response = this.adamRepository.Credit(creditEventTransaction, adamSettings, username);

            if (response == AdamResponse.AdamDown)
            {
                this.eventRepository.CurrentUser = username;
                this.eventRepository.InsertCreditEventTransaction(creditEventTransaction);
                // update job here to show that the job has been actioned
                this.jobRepository.SetJobToSubmittedStatus(job.Id);

                return response;
            }

            using (var transactionScope = new TransactionScope())
            {
                this.jobRepository.ResolveJobAndJobDetails(job.Id);
                this.userRepository.UnAssignJobToUser(job.Id);
                this.eventRepository.RemovedPendingCredit(job.Id);

                transactionScope.Complete();

                return AdamResponse.Success;
            }
        }
        
        public void Grn(GrnEvent grnEvent, int eventId, AdamSettings adamSettings, string username)
        {
            var adamResponse = this.adamRepository.Grn(grnEvent, adamSettings);

            this.MarkAsDone(eventId, adamResponse, username);
        }
        
        public AdamResponse Grn(GrnEvent grnEvent, AdamSettings adamSettings, string username)
        {
            var response = this.adamRepository.Grn(grnEvent, adamSettings);

            if (response == AdamResponse.AdamDown)
            {
                this.eventRepository.CurrentUser = username;
                this.eventRepository.InsertGrnEvent(grnEvent);
            }
            else
            {
                using (var transactionScope = new TransactionScope())
                {
                    //this.jobRepository.ResolveJobAndJobDetails(grnEvent.Id);
                    //this.userRepository.UnAssignJobToUser(grnEvent.Id);

                    transactionScope.Complete();

                    return AdamResponse.Success;
                }
            }

            return response;
        }

        public ProcessDeliveryActionResult ProcessDeliveryActions(List<DeliveryLine> lines, AdamSettings adamSettings, string username, int branchId)
        {
            var creditResult = this.CreditDeliveryLines(lines, adamSettings, username, branchId);

            return creditResult;
        }

        public ProcessDeliveryActionResult CreditDeliveryLines(List<DeliveryLine> lines, AdamSettings adamSettings, string username, int branchId)
        {
            var result = new ProcessDeliveryActionResult();

            var creditLines = GetDeliveryLinesByAction(lines, DeliveryAction.Credit);

            if (creditLines.Any())
            {
                var totalThresholdValue = creditLines.Sum(x => x.CreditValueForThreshold());

                // is the user allowed to credit this amount or does it need to go to the next threshold user
                var thresholdResponse = this.userThresholdService.CanUserCredit(username, totalThresholdValue);

                if (thresholdResponse.IsInError)
                {
                    result.ThresholdError = true;
                    result.ThresholdErrorMessage = thresholdResponse.ErrorMessage;
                }
                else
                {
                    if (!thresholdResponse.CanUserCredit)
                    {
                        this.userThresholdService.AssignPendingCredit(branchId, totalThresholdValue, creditLines[0].JobId, username);
                        result.CreditThresholdLimitReached = true;
                    }
                    else
                    {
                        result.AdamResponse = this.Credit(creditLines, adamSettings, username, branchId);
                    }
                }
            }

            return result;
        }

        private static IList<DeliveryLine> GetDeliveryLinesByAction(IList<DeliveryLine> deliveryLines, DeliveryAction action)
        {
            var lines = new List<DeliveryLine>();

            foreach (var line in deliveryLines)
            {
                if ((DeliveryAction)line.ShortsActionId == action)
                    lines.Add(line);
                else
                {
                    foreach (var damage in line.Damages)
                    {
                        if ((DeliveryAction)damage.DamageActionId == action && !lines.Contains(line))
                        {
                            lines.Add(line);
                        }
                    }
                }
            }

            return lines;
        }

        public void Pod(PodEvent podEvent, int eventId, AdamSettings adamSettings, string username)
        {
            var adamResponse = this.adamRepository.Pod(podEvent, adamSettings);

            this.MarkAsDone(eventId, adamResponse, username);
        }


        private void MarkAsDone(int eventId, AdamResponse response, string username)
        {
            if (response == AdamResponse.Success)
            {
                this.eventRepository.CurrentUser = username;
                this.eventRepository.MarkEventAsProcessed(eventId);
            }
        }
    }
}