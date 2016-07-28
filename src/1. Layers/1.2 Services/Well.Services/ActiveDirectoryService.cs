namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices.AccountManagement;
    using System.Linq;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Services.Contracts;

    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly ILogger logger;

        public ActiveDirectoryService(ILogger logger)
        {
            this.logger = logger;
        }

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

        public User GetUser(string username)
        {
            var parts = username.Split('\\');

            var domain = parts[0];

            var nameParts = parts[1].Split('.');
            var firstname = nameParts[0];
            var surname = nameParts[1];

            return this.Search(domain, firstname, surname, username);
        }

        public User GetUser(string username, string domain)
        {
            var nameParts = username.Split(' ');

            var firstname = nameParts[0];
            var surname = nameParts[1];

            var user = this.Search(domain, firstname, surname, username);

            user.IdentityName = $"{domain}\\{firstname}.{surname}";

            return user;
        }

        private void AddResult(UserPrincipal userPrincipal, string domain, List<User> users)
        {
            var search = new PrincipalSearcher(userPrincipal);

            foreach (var result in search.FindAll())
            {
                users.Add(new User { Name = result.Name, JobDescription = result.Description, Domain = domain });
            }
        }

        private User Search(string domain, string firstname, string surname, string username)
        {
            var context = new PrincipalContext(ContextType.Domain, domain);

            var userPrincipal = new UserPrincipal(context) { GivenName = firstname, Surname = surname };
            var search = new PrincipalSearcher(userPrincipal);

            var results = search.FindAll();

            if (results.Count() == 1)
            {
                var result = results.ToList()[0];

                return new User
                {
                    Name = result.Name,
                    IdentityName = username,
                    JobDescription = result.Description,
                    Domain = domain
                };
            }

            this.logger.LogDebug($"Error when trying to add a new user for {username}");

            if (results.Count() > 1) throw new ApplicationException($"More than one result returned when trying to add a new user for {username}");
            if (!results.Any()) throw new ApplicationException($"No result returned when trying to add a new user for {username}");

            return null;
        }
    }
}