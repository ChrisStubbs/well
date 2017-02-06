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

    public class EventProcessor
    {
        private readonly IExceptionEventRepository exceptionEventRepository;
        private readonly IDeliveryLineActionService deliveryLineActionService;
        private readonly ILogger logger;
        private readonly IEventLogger eventLogger;

        public EventProcessor(IContainer container)
        {
            this.exceptionEventRepository = container.GetInstance<IExceptionEventRepository>();
            this.deliveryLineActionService = container.GetInstance<IDeliveryLineActionService>();
            this.logger = container.GetInstance<ILogger>();
            this.eventLogger = container.GetInstance<IEventLogger>();
        }

        public void Process()
        {
            this.eventLogger.TryWriteToEventLog(EventSource.WellTaskRunner, "Processing ADAM tasks...", 5655, EventLogEntryType.Information);

            var username = "Event Processor";
            var eventsToProcess = this.exceptionEventRepository.GetAllUnprocessed();

            this.logger.LogDebug("Starting Well Adam Events!");

            foreach (var eventToProcess in eventsToProcess)
            {
                if (eventToProcess.DateCanBeProcessed < DateTime.Now)
                {
                    switch (eventToProcess.EventAction)
                    {
                        case EventAction.CreditTransaction:
                            var creditEventTransaction = JsonConvert.DeserializeObject<CreditTransaction>(eventToProcess.Event);
                            this.deliveryLineActionService.CreditTransaction(creditEventTransaction, eventToProcess.Id,
                                GetAdamSettings(creditEventTransaction.BranchId), username);
                            break;
                        case EventAction.Grn:
                            var grnEvent = JsonConvert.DeserializeObject<GrnEvent>(eventToProcess.Event);
                            this.deliveryLineActionService.Grn(grnEvent, eventToProcess.Id,
                                GetAdamSettings(grnEvent.BranchId), username);
                            break;
                        case EventAction.Pod:
                            var podTransaction = JsonConvert.DeserializeObject<PodTransaction>(eventToProcess.Event);
                            this.deliveryLineActionService.Pod(podTransaction, eventToProcess.Id, GetAdamSettings(podTransaction.BranchId), username);
                            break;
                    }
                }
            }

            this.logger.LogDebug("Finished Well Adam Events!");
        }

        private AdamSettings GetAdamSettings(int branchId)
        {
            return AdamSettingsFactory.GetAdamSettings((Branch)branchId);
        }
    }
}