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
        private readonly IExceptionEventService exceptionEventService;
        private readonly ILogger logger;

        public EventProcessor(IContainer container)
        {
            this.exceptionEventRepository = container.GetInstance<IExceptionEventRepository>();
            this.exceptionEventService = container.GetInstance<IExceptionEventService>();
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
                        //case EventAction.Credit:
                        //    var creditEvent = JsonConvert.DeserializeObject<CreditEvent>(eventToProcess.Event);
                        //    this.exceptionEventService.Credit(creditEvent, eventToProcess.Id,
                        //        GetAdamSettings(creditEvent.BranchId), username);
                        //    break;
                        case EventAction.CreditTransaction:
                            var creditEventTransaction = JsonConvert.DeserializeObject<CreditEventTransaction>(eventToProcess.Event);
                            this.exceptionEventService.CreditEventTransaction(creditEventTransaction, eventToProcess.Id,
                                GetAdamSettings(creditEventTransaction.BranchId), username);
                            break;
                        case EventAction.CreditAndReorder:
                            var creditReorderEvent =
                                JsonConvert.DeserializeObject<CreditReorderEvent>(eventToProcess.Event);
                            this.exceptionEventService.CreditReorder(creditReorderEvent, eventToProcess.Id,
                                GetAdamSettings(creditReorderEvent.BranchId), username);
                            break;
                        case EventAction.Reject:
                            var rejectEvent = JsonConvert.DeserializeObject<RejectEvent>(eventToProcess.Event);
                            this.exceptionEventService.Reject(rejectEvent, eventToProcess.Id,
                                GetAdamSettings(rejectEvent.BranchId), username);
                            break;
                        case EventAction.ReplanInRoadnet:
                            var roadnetEvent = JsonConvert.DeserializeObject<RoadnetEvent>(eventToProcess.Event);
                            this.exceptionEventService.ReplanRoadnet(roadnetEvent, eventToProcess.Id,
                                GetAdamSettings(roadnetEvent.BranchId), username);
                            break;
                        case EventAction.ReplanInTranSend:
                            var transcendEvent = JsonConvert.DeserializeObject<TranscendEvent>(eventToProcess.Event);
                            this.exceptionEventService.ReplanTranscend(transcendEvent, eventToProcess.Id,
                                GetAdamSettings(transcendEvent.BranchId), username);
                            break;
                        case EventAction.ReplanInTheQueue:
                            var queueEvent = JsonConvert.DeserializeObject<QueueEvent>(eventToProcess.Event);
                            this.exceptionEventService.ReplanQueue(queueEvent, eventToProcess.Id,
                                GetAdamSettings(queueEvent.BranchId), username);
                            break;
                        case EventAction.Grn:
                            var grnEvent = JsonConvert.DeserializeObject<GrnEvent>(eventToProcess.Event);
                            this.exceptionEventService.Grn(grnEvent, eventToProcess.Id,
                                GetAdamSettings(grnEvent.BranchId), username);
                            break;
                        case EventAction.Pod:
                            var podEvent = JsonConvert.DeserializeObject<PodEvent>(eventToProcess.Event);
                            this.exceptionEventService.Pod(podEvent, eventToProcess.Id, GetAdamSettings(podEvent.BranchId), username);
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