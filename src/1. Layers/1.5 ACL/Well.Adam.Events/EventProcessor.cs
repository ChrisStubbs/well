namespace PH.Well.Adam.Events
{
    using System;
    using Newtonsoft.Json;

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
        private readonly IDeliveryLineActionService exceptionEventService;
        private readonly ILogger logger;

        public EventProcessor(IContainer container)
        {
            this.exceptionEventRepository = container.GetInstance<IExceptionEventRepository>();
            this.exceptionEventService = container.GetInstance<IDeliveryLineActionService>();
            this.logger = container.GetInstance<ILogger>();
        }

        public void Process()
        {
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
                            this.exceptionEventService.CreditTransaction(creditEventTransaction, eventToProcess.Id,
                                GetAdamSettings(creditEventTransaction.BranchId), username);
                            break;
                        case EventAction.Grn:
                            var grnEvent = JsonConvert.DeserializeObject<GrnEvent>(eventToProcess.Event);
                            this.exceptionEventService.Grn(grnEvent, eventToProcess.Id,
                                GetAdamSettings(grnEvent.BranchId), username);
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