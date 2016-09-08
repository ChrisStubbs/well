namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;

    public interface IUserRepository : IRepository<User, int>
    {
        User GetByIdentity(string identity);

        IEnumerable<User> GetByBranchId(int branchId);

        void AssignJobToUser(int userId, int jobId);

        void UnAssignJobToUser(int jobId);
    }
}