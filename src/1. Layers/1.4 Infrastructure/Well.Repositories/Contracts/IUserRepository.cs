namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;

    public interface IUserRepository : IRepository<User, int>
    {
        //User GetById(int id);

        User GetByIdentity(string identity);

        User GetByIdentity(string identity, string connectionString);

        User GetByName(string name);

        IEnumerable<User> GetByBranchId(int branchId);

        void AssignJobToUser(int userId, int jobId);

        void UnAssignJobToUser(int jobId);

        IEnumerable<UserJob> GetUserJobsByJobIds(IEnumerable<int> jobIds);

        IEnumerable<User> Get(int? id = null, string identity = null, string name = null, int? creditThresholdId = null, int? branchId = null,string connectionString = null);

        void Save(User user, string connectionString);
    }
}