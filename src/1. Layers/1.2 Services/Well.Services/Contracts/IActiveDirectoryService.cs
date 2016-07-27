namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Domain;

    public interface IActiveDirectoryService
    {
        IEnumerable<User> FindUsers(string name, string domains);
    }
}