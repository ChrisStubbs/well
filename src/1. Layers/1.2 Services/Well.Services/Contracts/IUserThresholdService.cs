namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain.ValueObjects;

    public interface IUserThresholdService
    {
        ThresholdResponse CanUserCredit(decimal creditValue);

        string AssignPendingCredit(int branchId, decimal totalThresholdAmount, int jobId);
    }
}