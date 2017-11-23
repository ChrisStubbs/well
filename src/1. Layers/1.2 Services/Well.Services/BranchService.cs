using System.Linq;
using PH.Well.Common.Contracts;

namespace PH.Well.Services
{
    using System;
    using System.Transactions;

    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class BranchService : IBranchService
    {
        private readonly IUserRepository userRepository;

        private readonly IBranchRepository branchRepository;

        private readonly IActiveDirectoryService activeDirectoryService;

        private readonly IUserNameProvider userNameProvider;
        private readonly IDbMultiConfiguration connections;
        private readonly IUserService userService;

        public BranchService(IUserRepository userRepository,
            IBranchRepository branchRepository,
            IActiveDirectoryService activeDirectoryService,
            IUserNameProvider userNameProvider,
            IDbMultiConfiguration connections,
            IUserService userService)
        {
            this.userRepository = userRepository;
            this.branchRepository = branchRepository;
            this.activeDirectoryService = activeDirectoryService;
            this.userNameProvider = userNameProvider;
            this.connections = connections;
            this.userService = userService;
        }

        public void SaveBranchesForUser(Branch[] branches)
        {
            var identityName = userNameProvider.GetUserName();
            var user = this.userRepository.GetByIdentity(identityName);
            Action<Branch[], User, string> saveUser = SaveBranchesForExistingUser;

            if (user == null)
            {
                user = this.activeDirectoryService.GetUser(identityName);
                saveUser = SaveBranchesForNewUser;
                if (user == null) throw new ApplicationException($"User not found in active directory {identityName}");
            }

            SaveBranchesForUser(branches, saveUser, user);
        }
        
        public void SaveBranchesOnBehalfOfAUser(Branch[] branches, string username, string domain)
        {
            username = username.Replace('-', ' ');
            var user = this.userRepository.GetByName(username);
            Action<Branch[], User, string> saveUser = SaveBranchesForExistingUser;

            if (user == null)
            {
                user = this.activeDirectoryService.GetUser(username, domain);
                if (user == null) throw new ApplicationException($"User not found in active directory {username}");
                saveUser = SaveBranchesForNewUser;
            }

            SaveBranchesForUser(branches, saveUser, user);

        }

        private void SaveBranchesForUser(Branch[] branches, Action<Branch[], User, string> saveUser, User user)
        {
            foreach (var connectionString in connections.ConnectionStrings)
            {
                using (var transactionScope = new TransactionScope())
                {
                    saveUser.Invoke(branches, user, connectionString);
                    transactionScope.Complete();
                }
            }
        }
        public void SaveBranchesForExistingUser(Branch[] branches, User user, string connectionString)
        {
            user = userService.CreateUserIfNotExists(user, connectionString);
            this.branchRepository.DeleteUserBranches(user, connectionString);
            this.branchRepository.SaveBranchesForUser(branches, user, connectionString);
        }

        public void SaveBranchesForNewUser(Branch[] branches, User newUser, string connectionString)
        {
            newUser = userService.CreateUserIfNotExists(newUser, connectionString);
            this.branchRepository.SaveBranchesForUser(branches, newUser, connectionString);
        }

        /// <summary>
        /// Return a comma-separated list of selected branches
        /// If all selected return "All branches"
        /// If 6 or less branches selected, return their full names
        /// If more than 6 selected return their 3-letter abbreviations
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public string GetUserBranchesFriendlyInformation(string username)
        {
            var allBranchesCount = this.branchRepository.GetAllValidBranches().Count();
            var branches = this.branchRepository.GetBranchesForUser(username).Select(x => x.Name).ToList();
            string friendlyString = "";

            if (branches.Count() == allBranchesCount)
            {
                friendlyString = "All branches selected";
            }
            else if (branches.Count() <= 6)
            {
                friendlyString = string.Join(", ", branches);
            }
            else
            {
                friendlyString = string.Join(", ", branches.Select(x => x.Substring(0, 3)));
            }

            return friendlyString;
        }
    }
}
