namespace PH.Well.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;
    using PH.Well.Common.Contracts;

    public class UserThresholdService : IUserThresholdService
    {
        private readonly ICreditThresholdRepository creditThresholdRepository;
        private readonly IUserRepository userRepository;
        private readonly IUserNameProvider userNameProvider;

        public UserThresholdService(ICreditThresholdRepository creditThresholdRepository,
            IUserRepository userRepository,
            IUserNameProvider userNameProvider)
        {
            this.creditThresholdRepository = creditThresholdRepository;
            this.userRepository = userRepository;
            this.userNameProvider = userNameProvider;
        }

        public ThresholdResponse CanUserCredit(decimal creditValue)
        {
            var username = this.userNameProvider.GetUserName();
            var response = new ThresholdResponse();
            var user = this.userRepository.GetByIdentity(username);

            if (user == null) 
            {
                response.IsInError = true;
                response.ErrorMessage = $"User not found ({username})";
            }

            if (user.ThresholdLevelId == null)
            {
                response.IsInError = true;
                response.ErrorMessage = $"You must be assigned a threshold level before crediting.";
                return response;
            }

            var threshold =
                this.creditThresholdRepository.GetAll().FirstOrDefault(x => x.ThresholdLevelId == user.ThresholdLevelId);

            if (threshold == null) 
            {
                response.IsInError = true;
                response.ErrorMessage = $"Threshold not found with id ({user.ThresholdLevelId})";

                return response;
            }

            if (creditValue <= threshold.Threshold) response.CanUserCredit = true;

            return response;
        }

        public string AssignPendingCredit(int branchId, decimal totalThresholdAmount, int jobId)
        {
            var branchSpecificThresholds = this.creditThresholdRepository.GetByBranch(branchId);

            if (!this.ApplyThreshold(branchSpecificThresholds, ThresholdLevel.Level2, branchId, totalThresholdAmount, jobId))
            {
                if (!this.ApplyThreshold(branchSpecificThresholds, ThresholdLevel.Level1, branchId, totalThresholdAmount, jobId))
                {
                    return $"There are no levels that can handle the credit value of ({totalThresholdAmount}) for branch ({branchId})";
                }
            }
            return string.Empty;
        }

        private bool ApplyThreshold(IEnumerable<CreditThreshold> branchThresholds, ThresholdLevel level, int branchId, decimal totalThresholdAmount, int jobId)
        {
            var threshold = branchThresholds.FirstOrDefault(x => x.ThresholdLevel == level);

            if (threshold == null)
                return false;

            if (totalThresholdAmount <= threshold.Threshold)
            {
                creditThresholdRepository.PendingCreditInsert(jobId);
                userRepository.UnAssignJobToUser(jobId);

                return true;
            }

            return false;
        }
    }
}