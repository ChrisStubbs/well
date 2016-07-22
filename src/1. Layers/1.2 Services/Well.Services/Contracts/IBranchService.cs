namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain;

    public interface IBranchService
    {
        void SaveBranchesForUser(Branch[] branches, string username);

        string GetUserBranchesFriendlyInformation(string username);
    }
}