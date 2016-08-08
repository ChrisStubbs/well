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
            var eventsToProcess = this.exceptionEventRepository.GetAllUnprocessed();

            foreach (var eventToProcess in eventsToProcess)
            {
                switch (eventToProcess.ExceptionAction)
                {
                    case ExceptionAction.Credit:
                        var creditEvent = JsonConvert.DeserializeObject<CreditEvent>(eventToProcess.Event);
                        this.exceptionEventService.Credit(creditEvent, GetAdamSettings(creditEvent.BranchId));
                        break;
                    case ExceptionAction.CreditAndReorder:
                        var creditReorderEvent = JsonConvert.DeserializeObject<CreditReorderEvent>(eventToProcess.Event);
                        this.exceptionEventService.CreditReorder(creditReorderEvent, GetAdamSettings(creditReorderEvent.BranchId));
                        break;
                    case ExceptionAction.Reject:
                        var rejectEvent = JsonConvert.DeserializeObject<RejectEvent>(eventToProcess.Event);
                        this.exceptionEventService.Reject(rejectEvent, GetAdamSettings(rejectEvent.BranchId));
                        break;
                    case ExceptionAction.ReplanInRoadnet:
                        var roadnetEvent = JsonConvert.DeserializeObject<RoadnetEvent>(eventToProcess.Event);
                        this.exceptionEventService.ReplanRoadnet(roadnetEvent, GetAdamSettings(roadnetEvent.BranchId));
                        break;
                    case ExceptionAction.ReplanInTranscend:
                        var transcendEvent = JsonConvert.DeserializeObject<TranscendEvent>(eventToProcess.Event);
                        this.exceptionEventService.ReplanTranscend(transcendEvent, GetAdamSettings(transcendEvent.BranchId));
                        break;
                    case ExceptionAction.ReplanInTheQueue:
                        var queueEvent = JsonConvert.DeserializeObject<QueueEvent>(eventToProcess.Event);
                        this.exceptionEventService.ReplanQueue(queueEvent, GetAdamSettings(queueEvent.BranchId));
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