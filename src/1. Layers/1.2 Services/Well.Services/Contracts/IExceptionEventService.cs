namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;

    public interface IExceptionEventService
    {
        void Credit(CreditEvent creditEvent, AdamSettings adamSettings);

        AdamResponse Credit(CreditEvent creditEvent, AdamSettings adamSettings, string username);
    }
}