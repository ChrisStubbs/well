namespace PH.Well.Services
{
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class ExceptionEventService : IExceptionEventService
    {
        private readonly IAdamRepository adamRepository;

        private readonly IExceptionEventRepository eventRepository;

        public ExceptionEventService(IAdamRepository adamRepository, IExceptionEventRepository eventRepository)
        {
            this.adamRepository = adamRepository;
            this.eventRepository = eventRepository;
        }

        public void Credit(CreditEvent creditEvent, AdamSettings adamSettings)
        {
            this.adamRepository.CreditInvoice(creditEvent, adamSettings);
        }

        public AdamResponse Credit(CreditEvent creditEvent, AdamSettings adamSettings, string username)
        {
            var response = this.adamRepository.CreditInvoice(creditEvent, adamSettings);

            if (response == AdamResponse.AdamDown)
            {
                this.eventRepository.CurrentUser = username;
                this.eventRepository.InsertCreditEvent(creditEvent);
            }

            return response;
        }
    }
}