namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;

    public interface IBranchRepository : IRepository<Branch, int>
    {
        IEnumerable<Branch> GetAll();

        IEnumerable<Branch> GetAllValidBranches();

        void DeleteUserBranches(User user, string connectionString);

        void SaveBranchesForUser(IEnumerable<Branch> branches, User user, string connectionString);

        IEnumerable<Branch> GetBranchesForUser(string username);

        IEnumerable<Branch> GetBranchesForSeasonalDate(int seasonalDateId);

        int GetBranchIdForJob(int jobId);

        int GetBranchIdForStop(int stopId);
    }
}