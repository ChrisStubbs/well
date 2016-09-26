namespace PH.Well.Adam.Events
{
    using Newtonsoft.Json;

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
        
        public EventProcessor(IContainer container)
        {
            this.exceptionEventRepository = container.GetInstance<IExceptionEventRepository>();
            this.exceptionEventService = container.GetInstance<IExceptionEventService>();
        }

        public void Process()
        {
            var username = "Event Processor";
            var eventsToProcess = this.exceptionEventRepository.GetAllUnprocessed();

            foreach (var eventToProcess in eventsToProcess)
            {
                switch (eventToProcess.ExceptionAction)
                {
                    case ExceptionAction.Credit:
                        var creditEvent = JsonConvert.DeserializeObject<CreditEvent>(eventToProcess.Event);
                        this.exceptionEventService.Credit(creditEvent, eventToProcess.Id, GetAdamSettings(creditEvent.BranchId), username);
                        break;
                    case ExceptionAction.CreditAndReorder:
                        var creditReorderEvent = JsonConvert.DeserializeObject<CreditReorderEvent>(eventToProcess.Event);
                        this.exceptionEventService.CreditReorder(creditReorderEvent, eventToProcess.Id, GetAdamSettings(creditReorderEvent.BranchId), username);
                        break;
                    case ExceptionAction.Reject:
                        var rejectEvent = JsonConvert.DeserializeObject<RejectEvent>(eventToProcess.Event);
                        this.exceptionEventService.Reject(rejectEvent, eventToProcess.Id, GetAdamSettings(rejectEvent.BranchId), username);
                        break;
                    case ExceptionAction.ReplanInRoadnet:
                        var roadnetEvent = JsonConvert.DeserializeObject<RoadnetEvent>(eventToProcess.Event);
                        this.exceptionEventService.ReplanRoadnet(roadnetEvent, eventToProcess.Id, GetAdamSettings(roadnetEvent.BranchId), username);
                        break;
                    case ExceptionAction.ReplanInTranSend:
                        var transcendEvent = JsonConvert.DeserializeObject<TranscendEvent>(eventToProcess.Event);
                        this.exceptionEventService.ReplanTranscend(transcendEvent, eventToProcess.Id, GetAdamSettings(transcendEvent.BranchId), username);
                        break;
                    case ExceptionAction.ReplanInTheQueue:
                        var queueEvent = JsonConvert.DeserializeObject<QueueEvent>(eventToProcess.Event);
                        this.exceptionEventService.ReplanQueue(queueEvent, eventToProcess.Id, GetAdamSettings(queueEvent.BranchId), username);
                        break;
                }
            }
        }

        private AdamSettings GetAdamSettings(int branchId)
        {
            return AdamSettingsFactory.GetAdamSettings((Branch)branchId);
        }
    }
}