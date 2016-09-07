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

        public BranchService(IUserRepository userRepository, IBranchRepository branchRepository, IActiveDirectoryService activeDirectoryService)
        {
            this.userRepository = userRepository;
            this.branchRepository = branchRepository;
            this.activeDirectoryService = activeDirectoryService;
        }

        public void SaveBranchesForUser(Branch[] branches, string username)
        {
            this.branchRepository.CurrentUser = username;
            this.userRepository.CurrentUser = username;

            var user = this.userRepository.GetByIdentity(username);

            if (user == null)
            {
                var newUser = this.activeDirectoryService.GetUser(username);

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

        public void SaveBranchesOnBehalfOfAUser(Branch[] branches, string username, string identityName, string domain)
        {
            this.branchRepository.CurrentUser = identityName;
            this.userRepository.CurrentUser = identityName;

            username = username.Replace('-', ' ');

            var user = this.userRepository.GetByIdentity(username);

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

        public string GetUserBranchesFriendlyInformation(string username)
        {
            var branches = this.branchRepository.GetBranchesForUser(username);
            var friendlyString = new StringBuilder();

            foreach (var branch in branches)
            {
                friendlyString.Append(branch.Name.Substring(0, 3) + ", ");
            }

            var output = friendlyString.ToString();

            output = output.TrimEnd(',', ' ');

            return output;
        }
    }
}