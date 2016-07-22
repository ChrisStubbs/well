namespace PH.Well.Services
{
    using System.Text;
    using System.Transactions;

    using PH.Well.Domain;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class BranchService : IBranchService
    {
        private readonly IUserRepository userRepository;

        private readonly IBranchRepository branchRepository;

        public BranchService(IUserRepository userRepository, IBranchRepository branchRepository)
        {
            this.userRepository = userRepository;
            this.branchRepository = branchRepository;
        }

        public void SaveBranchesForUser(Branch[] branches, string username)
        {
            this.userRepository.CurrentUser = username;
            this.branchRepository.CurrentUser = username;
            var user = this.userRepository.GetByName(username);

            if (user == null)
            {
                var newUser = new User { Name = username };

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