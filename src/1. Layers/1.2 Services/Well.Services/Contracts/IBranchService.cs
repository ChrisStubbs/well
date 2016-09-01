namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain;

    public interface IBranchService
    {
        void SaveBranchesForUser(Branch[] branches, string username);

        void SaveBranchesOnBehalfOfAUser(Branch[] branches, string username, string identityName, string domain);

        string GetUserBranchesFriendlyInformation(string username);
    }
}