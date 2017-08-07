namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;

    public interface IUserRepository : IRepository<User, int>
    {
        User GetById(int id);
        User GetByIdentity(string identity);
        User GetByName(string name);

        IEnumerable<User> GetByBranchId(int branchId);

        void AssignJobToUser(int userId, int jobId);

        void UnAssignJobToUser(int jobId);

        IEnumerable<UserJob> GetUserJobsByJobIds(IEnumerable<int> jobIds);
    }
}