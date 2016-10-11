namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain.ValueObjects;

    public interface IUserThresholdService
    {
        bool CanUserCredit(string username, int creditValue);

        void AssignPendingCredit(CreditEvent creditEvent, string originator);
    }
}