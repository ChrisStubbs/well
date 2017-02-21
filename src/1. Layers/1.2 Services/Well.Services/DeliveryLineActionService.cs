﻿using PH.Well.Common.Contracts;

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
            var adamResponse = this.adamRepository.Grn(grnEvent, adamSettings);

            this.MarkAsDone(eventId, adamResponse);
        }

        public ProcessDeliveryActionResult ProcessDeliveryActions(List<DeliveryLine> lines, AdamSettings adamSettings, int branchId)
        {
            var groupdLines = Enum.GetValues(typeof(DeliveryAction)).Cast<DeliveryAction>()
            .Select(p => new
            {
                key = p,
                values = GetDeliveryLinesByAction(lines, p)
            })
            .ToDictionary(p => p.key, v => v.values);

            using (var transactionScope = new TransactionScope())
            {
                var results = allActionHandlers
                    .OrderBy(p=> p.Action)
                    .Select(p => p.Execute(delAction => groupdLines[delAction], adamSettings, branchId))
                    .ToList();

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

        public void Pod(PodEvent podEvent, int eventId, AdamSettings adamSettings)
        {
            var adamResponse = this.adamRepository.Pod(podEvent, adamSettings);

            this.MarkAsDone(eventId, adamResponse);
        }

        private void MarkAsDone(int eventId, AdamResponse response)
        {
            if (response == AdamResponse.Success)
            {
                //////this.eventRepository.CurrentUser = username;
                this.eventRepository.MarkEventAsProcessed(eventId);
            }
        }
    }
}