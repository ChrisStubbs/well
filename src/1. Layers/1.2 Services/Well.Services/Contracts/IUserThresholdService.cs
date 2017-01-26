namespace PH.Well.Services.Contracts
{
    public interface IUserThresholdService
    {
        bool CanUserCredit(string username, decimal creditValue);

        void AssignPendingCredit(int branchId, decimal totalThresholdAmount, int jobId, string originator);
    }
}