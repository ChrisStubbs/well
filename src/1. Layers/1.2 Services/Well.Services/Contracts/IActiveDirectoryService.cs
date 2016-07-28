namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;

    public interface IActiveDirectoryService
    {
        IEnumerable<User> FindUsers(string name, string domains);

        User GetUser(string username);

        User GetUser(string username, string domain);
    }
}