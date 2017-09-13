using System.Linq;
using PH.Well.Common.Contracts;

namespace PH.Well.Services
{
    using System;
    using System.Text;
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

        public BranchService(IUserRepository userRepository, 
            IBranchRepository branchRepository, 
            IActiveDirectoryService activeDirectoryService, 
            IUserNameProvider userNameProvider)
        {
            this.userRepository = userRepository;
            this.branchRepository = branchRepository;
            this.activeDirectoryService = activeDirectoryService;
            this.userNameProvider = userNameProvider;
        }

        public void SaveBranchesForUser(Branch[] branches)
        {
            var identityName = userNameProvider.GetUserName();

            var user = this.userRepository.GetByIdentity(identityName);

            if (user == null)
            {
                var newUser = this.activeDirectoryService.GetUser(identityName);

                if (newUser == null) throw new ApplicationException($"User not found in active directory {identityName}");

                using (var transactionScope = new TransactionScope())
                {
                    this.userRepository.Save(newUser);
                    this.branchRepository.SaveBranchesForUser(branches, newUser);
                    transactionScope.Complete();
                }
            }
            else
            {
                using (var transactionScope = new TransactionScope())
                {
                    this.branchRepository.DeleteUserBranches(user);
                    this.branchRepository.SaveBranchesForUser(branches, user);
                    transactionScope.Complete();
                }
            }
        }

        public void SaveBranchesOnBehalfOfAUser(Branch[] branches, string username, string domain)
        {
            username = username.Replace('-', ' ');

            var user = this.userRepository.GetByName(username);

            if (user == null)
            {
                var newUser = this.activeDirectoryService.GetUser(username, domain);

                if (newUser == null) throw new ApplicationException($"User not found in active directory {username}");

                using (var transactionScope = new TransactionScope())
                {
                    this.userRepository.Save(newUser);
                    this.branchRepository.SaveBranchesForUser(branches, newUser);
                    transactionScope.Complete();
                }
            }
            else
            {
                using (var transactionScope = new TransactionScope())
                {
                    this.branchRepository.DeleteUserBranches(user);
                    this.branchRepository.SaveBranchesForUser(branches, user);
                    transactionScope.Complete();
                }
            }
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
            var branches = this.branchRepository.GetBranchesForUser(username).Select(x=>x.Name).ToList();
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
                friendlyString = string.Join(", ", branches.Select(x => x.Substring(0,3)));
            }

            return friendlyString;
        }
    }
}
