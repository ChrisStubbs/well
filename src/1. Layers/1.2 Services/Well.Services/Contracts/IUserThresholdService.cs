namespace PH.Well.Services.Contracts
{
    using System;
    using Domain;
    using PH.Well.Domain.ValueObjects;

    public interface IUserThresholdService
    {
        ThresholdResponse CanUserCredit(decimal creditValue);

        string AssignPendingCredit(int branchId, decimal totalThresholdAmount, int jobId);

        Decimal GetUserCreditThresholdValue();

        bool UserHasRequiredCreditThreshold(Job job);

        void SetThresholdLevelAllDatabases(string userName, int creditThresholdId);

        CreditThreshold GetCreditThreshold(string userName);
    }
}