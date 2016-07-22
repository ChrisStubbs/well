namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;

    public interface IBranchRepository : IRepository<Branch, int>
    {
        IEnumerable<Branch> GetAll();

        void DeleteUserBranches(User user);

        void SaveBranchesForUser(IEnumerable<Branch> branches, User user);

        IEnumerable<Branch> GetBranchesForUser(string username);
    }
}