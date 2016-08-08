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
                        var adamSettings = AdamSettingsFactory.GetAdamSettings((Branch)creditEvent.BranchId);
                        this.exceptionEventService.Credit(creditEvent, adamSettings);
                        break;
                }
            }
        }
    }
}