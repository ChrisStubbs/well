namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain.ValueObjects;

    public interface IUserThresholdService
    {
        ThresholdResponse CanUserCredit(string username, decimal creditValue);

        void AssignPendingCredit(int branchId, decimal totalThresholdAmount, int jobId, string originator);
    }
}