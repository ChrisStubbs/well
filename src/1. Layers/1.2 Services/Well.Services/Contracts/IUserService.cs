namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface IUserService
    {
        User GetByName(string name, string domainsToSearch);

        IList<User> GetByBranchId(int branchId);

        User CreateNewUserByIdentityOnAllDatabases(string userIdentity);

        IList<User> Get();

        User CreateUserIfNotExists(User user, string connectionString);
    }
}