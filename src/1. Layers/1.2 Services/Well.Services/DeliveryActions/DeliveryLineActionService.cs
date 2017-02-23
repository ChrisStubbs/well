using PH.Well.Common.Contracts;

namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Transactions;

    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class DeliveryLineActionService : IDeliveryLineActionService
    {
        private readonly IAdamRepository adamRepository;

        private readonly IJobRepository jobRepository;
        private readonly IUserRepository userRepository;
        private readonly IUserThresholdService userThresholdService;
        private readonly IExceptionEventRepository eventRepository;
        private readonly IEnumerable<IDeliveryLinesAction> allActionHandlers;
        private readonly IUserNameProvider userNameProvider;

        public DeliveryLineActionService(
            IAdamRepository adamRepository,
            IJobRepository jobRepository,
            IUserRepository userRepository,
            IUserThresholdService userThresholdService,
            IExceptionEventRepository eventRepository,
            IEnumerable<IDeliveryLinesAction> allActionHandlers,
            IUserNameProvider userNameProvider)
        {
            this.adamRepository = adamRepository;
            this.jobRepository = jobRepository;
            this.userRepository = userRepository;
            this.userThresholdService = userThresholdService;
            this.eventRepository = eventRepository;
            this.allActionHandlers = allActionHandlers;
            this.userNameProvider = userNameProvider;
        }

        public void CreditTransaction(CreditTransaction creditTransaction, int eventId, AdamSettings adamSettings)
        {
            var adamResponse = this.adamRepository.Credit(creditTransaction, adamSettings);

            this.MarkAsDone(eventId, adamResponse);
        }

        public void Grn(GrnEvent grnEvent, int eventId, AdamSettings adamSettings)
        {
            var adamResponse = this.adamRepository.Grn(grnEvent, adamSettings, username);

            this.MarkAsDone(eventId, adamResponse);
        }

        public ProcessDeliveryActionResult ProcessDeliveryActions(List<DeliveryLine> lines, AdamSettings adamSettings,
            string username, int branchId)
        {
            var groupdLines = Enum.GetValues(typeof(DeliveryAction)).Cast<DeliveryAction>()
                .Select(p => new
                {
                    key = p,
                    values = GetDeliveryLinesByAction(lines, p)
                })
                .ToDictionary(p => p.key, v => v.values);

            List<ProcessDeliveryActionResult> results = null;
            using (var transactionScope = new TransactionScope())
            {
                results = allActionHandlers
                    .OrderBy(p => p.Action)
                    .Select(p => p.Execute(delAction => groupdLines[delAction], adamSettings, branchId))
                    .ToList();

                transactionScope.Complete();


                return new ProcessDeliveryActionResult
                {
                    AdamIsDown = results.Any(p => p.AdamIsDown),
                    Warnings = results.SelectMany(p => p.Warnings).ToList()
                };

            }
        }

        private IList<DeliveryLine> GetDeliveryLinesByAction(IList<DeliveryLine> deliveryLines, DeliveryAction action)
        {
            return deliveryLines
                //get all lines that matches the action 
                .Where(p => (DeliveryAction)p.ShortsActionId == action)
                .Union
                (
                    deliveryLines
                        //only line not handle yet
                        .Where(p => (DeliveryAction)p.ShortsActionId != action)
                        //get damages with the action
                        .Where(p => p.Damages.Any(damage => (DeliveryAction)damage.DamageActionId == action))
                        .Select(p => p)
                )
                .ToList();
        }

        public void Pod(PodTransaction podTransaction, int eventId, AdamSettings adamSettings, string username)
        {
            var adamResponse = this.adamRepository.Pod(podTransaction, adamSettings);

            this.MarkAsDone(eventId, adamResponse);
            this.MarkPodAsResolved(podTransaction.JobId, adamResponse);
            
        }

        //public ProcessDeliveryPodActionResult ProcessDeliveryPodActions(List<DeliveryLine> lines, AdamSettings adamSettings, string username, int branchId)
        //{
        //    var podResult = this.PodDeliveryLines(lines, adamSettings, username, branchId);

        //    return podResult;
        //}

        //public ProcessDeliveryPodActionResult PodDeliveryLines(List<DeliveryLine> lines, AdamSettings adamSettings, string username, int branchId)
        //{
        //    var result = new ProcessDeliveryPodActionResult();

        //    var podLines = GetDeliveryLinesByAction(lines, DeliveryAction.Pod);

        //    if (podLines.Any())
        //    {
        //          result.AdamResponse = this.Pod(podLines, adamSettings, username, branchId);
        //    }

        //    return result;
        //}


        {
            if (response == AdamResponse.Success)
            {
                //////this.eventRepository.CurrentUser = username;
                this.eventRepository.MarkEventAsProcessed(eventId);
            }
        }

        private void MarkPodAsResolved(int jobId, AdamResponse response)
        {
            //TODO
            if (response == AdamResponse.Success)
            {
               this.jobRepository.ResolveJobAndJobDetails(jobId);
            }
        }

        private void MarkGrnAsComplete(int eventId, AdamResponse response, string username)
        {
            //TODO - prevent re-submission of GRN to ADAM
            //if (response == AdamResponse.Success)
            //{
            //    this.eventRepository.CurrentUser = username;
            //    this.eventRepository.MarkEventAsProcessed(eventId);
            //}
        }
    }
}