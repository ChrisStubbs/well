namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;
    using Common.Contracts;
    using Contracts;
    using Domain;
    using Repositories.Contracts;

    public class UserService : IUserService
    {
        private readonly IActiveDirectoryService activeDirectoryService;
        private readonly IUserRepository userRepository;
        private readonly IDbMultiConfiguration connections;
        private readonly IUserNameProvider userNameProvider;


        public UserService(IActiveDirectoryService activeDirectoryService,
            IUserRepository userRepository,
            IDbMultiConfiguration connections,
            IUserNameProvider userNameProvider)
        {
            this.activeDirectoryService = activeDirectoryService;
            this.userRepository = userRepository;
            this.connections = connections;
            this.userNameProvider = userNameProvider;
        }

        private User CreateNewUserByNameOnAllDatabases(string name, string domainsToSearch)
        {
            var usr = this.activeDirectoryService.FindUsers(name.Split(' ')[0], domainsToSearch)
                .ToList()
                .FirstOrDefault(p => p.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));

            if (usr == null)
            {
                return null;
            }
            CreateUserOnAllDatabases(usr);

            return usr;
        }

        public User GetByName(string name, string domainsToSearch)
        {
           return userRepository.GetByName(name) ??
                       CreateNewUserByNameOnAllDatabases(name, domainsToSearch);
        }


        public User CreateNewUserByIdentityOnAllDatabases(string userIdentity)
        {
            var user = this.activeDirectoryService.GetUser(userIdentity);

            CreateUserOnAllDatabases(user);

            return user;
        }

        public void CreateUserOnAllDatabases(User user)
        {
            using (var transaction = new TransactionScope())
            {
                foreach (string connectionString in connections.ConnectionStrings)
                {
                    CreateUserIfNotExists(user, connectionString);
                }
                transaction.Complete();
            }
        }

        public IList<User> GetByBranchId(int branchId)
        {
            return userRepository.GetByBranchId(branchId).ToList();
        }

        public IList<User> Get()
        {
            var data = this.userRepository.Get().ToList();
            var result = new List<User>(data.Count());
            var me = data.FirstOrDefault(p => p.IdentityName == userNameProvider.GetUserName());

            if (me != null)
            {
                result.Add(me);
            }

            result.AddRange(data
                .Where(p => p.IdentityName != userNameProvider.GetUserName())
                .OrderBy(p => p.Name));

            return result;
        }

        public User CreateUserIfNotExists(User user, string connectionString)
        {
            var existingUser = userRepository.GetByIdentity(user.IdentityName, connectionString);
             
            if (existingUser == null)
            {
                userRepository.Save(user, connectionString);
                return user;
            }

            return existingUser;
        }

       
    }
}