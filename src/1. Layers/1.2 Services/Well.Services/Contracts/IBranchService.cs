using System.Collections.Generic;

namespace PH.Well.Services.Contracts
{
    using PH.Well.Domain;

    public interface IBranchService
    {
        void SaveBranchesForUser(Branch[] branches, string username);

        void SaveBranchesOnBehalfOfAUser(Branch[] branches, string username, string identityName, string domain);

        string GetUserBranchesFriendlyInformation(string username);

        IEnumerable<User> GetUsersForBranch(int branchId);

        void AssignUserToJob(int userId, int jobId);
    }
}