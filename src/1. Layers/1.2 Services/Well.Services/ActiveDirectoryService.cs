namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.DirectoryServices.AccountManagement;

    using PH.Well.Domain;
    using PH.Well.Services.Contracts;

    public class ActiveDirectoryService : IActiveDirectoryService
    {
        public IEnumerable<User> FindUsers(string name, string domains)
        {
            var domainsToSearch = domains.Split(';');

            var users = new List<User>();

            foreach (var domain in domainsToSearch)
            {
                var context = new PrincipalContext(ContextType.Domain, domain);
                
                name = name.Trim();

                var firstNameSearch = new UserPrincipal(context) { GivenName = $"{name}*" };

                this.AddResult(firstNameSearch, domain, users);

                var lastNameSearch = new UserPrincipal(context) { Surname = $"{name}*" };

                this.AddResult(lastNameSearch, domain, users);
            }
            
            return users;
        }

        private void AddResult(UserPrincipal userPrincipal, string domain, List<User> users)
        {
            var search = new PrincipalSearcher(userPrincipal);

            foreach (var result in search.FindAll())
            {
                users.Add(new User { Name = $"{domain}\\{result.SamAccountName }" });
            }
        }
    }
}