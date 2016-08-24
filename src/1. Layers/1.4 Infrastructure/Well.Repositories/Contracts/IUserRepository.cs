using System.Collections.Generic;

namespace PH.Well.Repositories.Contracts
{
    using PH.Well.Domain;

    public interface IUserRepository : IRepository<User, int>
    {
        User GetByName(string name);
        IEnumerable<User> GetByBranchId(int branchId);
        void AssignJobToUser(int userId, int jobId);
    }
}