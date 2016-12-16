namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain.ValueObjects;

    public interface IUserThresholdService
    {
        bool CanUserCredit(string username, decimal creditValue);

        void AssignPendingCredit(CreditEvent creditEvent, string originator);

        Dictionary<int, string> RemoveCreditEventsThatDontHaveAThreshold(
            List<CreditEvent> creditEvents,
            string originator);
    }
}