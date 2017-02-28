namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain;

    public interface IBranchService
    {
        void SaveBranchesForUser(Branch[] branches);

        void SaveBranchesOnBehalfOfAUser(Branch[] branches, string username, string domain);

        string GetUserBranchesFriendlyInformation(string username);
    }
}