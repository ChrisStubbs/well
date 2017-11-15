using System.Linq;
using PH.Well.Domain;

namespace PH.Well.Adam.Events
{
    using System;
    using System.Diagnostics;

    using Newtonsoft.Json;

    using PH.Well.Common;
    using PH.Well.Common.Contracts;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services;
    using PH.Well.Services.Contracts;

    using StructureMap;

    public class EventProcessor : IEventProcessor
    {
        private readonly IExceptionEventRepository exceptionEventRepository;
        private readonly IDeliveryLineActionService deliveryLineActionService;
        private readonly ILogger logger;
        private readonly IEventLogger eventLogger;
        private IAdamRepository adamRepository;

        public EventProcessor(
            IExceptionEventRepository exceptionEventRepository,
            IDeliveryLineActionService deliveryLineActionService,
            ILogger logger,
            IEventLogger eventLogger,
            IAdamRepository adamRepository
            )
        {
            this.exceptionEventRepository = exceptionEventRepository;
            this.deliveryLineActionService = deliveryLineActionService;
            this.logger = logger;
            this.eventLogger = eventLogger;
            this.adamRepository = adamRepository;
        }

        public void Process()
        {
            this.eventLogger.TryWriteToEventLog(EventSource.WellTaskRunner, "Processing ADAM tasks...", EventId.EventProcessorLog, EventLogEntryType.Information);

            var username = "Event Processor";
            var eventsToProcess = this.exceptionEventRepository.GetAllUnprocessed();

            this.logger.LogDebug("Starting Well Adam Events!");

            // create all pod events for deliveries over 24 hours old

            foreach (var eventToProcess in eventsToProcess)
            {
                if (eventToProcess.DateCanBeProcessed < DateTime.Now)
                {
                    try
                    {
                        ProcessSingleEvent(eventToProcess);
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogError("Exception during ProcessSingleEvent:", ex);
                    }
                }
            }

            this.logger.LogDebug("Finished Well Adam Events!");
        }

        private void ProcessSingleEvent(ExceptionEvent eventToProcess)
        {
            switch (eventToProcess.EventAction)
            {
                case EventAction.Credit:
                    var creditEventTransaction = JsonConvert.DeserializeObject<CreditTransaction>(eventToProcess.Event);
                    this.deliveryLineActionService.CreditOrUpliftTransaction(creditEventTransaction, eventToProcess.Id,
                        GetAdamSettings(creditEventTransaction.BranchId));
                    break;
                case EventAction.Grn:
                    var grnEvent = JsonConvert.DeserializeObject<GrnEvent>(eventToProcess.Event);
                    this.deliveryLineActionService.Grn(grnEvent, eventToProcess.Id,
                        GetAdamSettings(grnEvent.BranchId));
                    break;
                case EventAction.Pod:
                    var podEvent = JsonConvert.DeserializeObject<PodEvent>(eventToProcess.Event);
                    this.deliveryLineActionService.Pod(podEvent, eventToProcess.Id, GetAdamSettings(podEvent.BranchId));
                    break;
                case EventAction.PodTransaction:
                    var podTransaction = JsonConvert.DeserializeObject<PodTransaction>(eventToProcess.Event);
                    this.deliveryLineActionService.PodTransaction(podTransaction, eventToProcess.Id,
                        GetAdamSettings(podTransaction.BranchId));
                    break;
                case EventAction.Amendment:
                    var amendmentTransaction = JsonConvert.DeserializeObject<AmendmentTransaction>(eventToProcess.Event);
                    this.deliveryLineActionService.AmendmentTransaction(amendmentTransaction, eventToProcess.Id,
                        GetAdamSettings(amendmentTransaction.BranchId));
                    break;
                case EventAction.GlobalUplift:
                    var upliftEvent =
                        JsonConvert.DeserializeObject<GlobalUpliftEvent>(eventToProcess.Event);
                    // Create transaction from event
                    var transaction = new GlobalUpliftTransaction(upliftEvent.Id, upliftEvent.BranchId,
                        upliftEvent.AccountNumber, upliftEvent.CreditReasonCode, upliftEvent.ProductCode,
                        upliftEvent.Quantity, upliftEvent.StartDate, upliftEvent.EndDate,
                        upliftEvent.WriteLine, upliftEvent.WriteHeader, upliftEvent.CsfNumber, upliftEvent.CustomerReference);

                    // Process event
                    var result = adamRepository.GlobalUplift(transaction, GetAdamSettings(transaction.BranchId));
                    if (result == AdamResponse.Success)
                    {
                        exceptionEventRepository.MarkEventAsProcessed(eventToProcess.Id);
                    }
                    else
                    {
                        // Delete event since it will be recreated in adam repository
                        exceptionEventRepository.Delete(eventToProcess.Id);
                    }
                    break;
                //TODO
                case EventAction.StandardUplift:
                    var upliftTransaction = JsonConvert.DeserializeObject<CreditTransaction>(eventToProcess.Event);
                    this.deliveryLineActionService.CreditOrUpliftTransaction(upliftTransaction, eventToProcess.Id,
                        GetAdamSettings(upliftTransaction.BranchId));
                    break;
            }
        }

        private AdamSettings GetAdamSettings(int branchId)
        {
            return AdamSettingsFactory.GetAdamSettings((Branch)branchId);
        }
    }
}