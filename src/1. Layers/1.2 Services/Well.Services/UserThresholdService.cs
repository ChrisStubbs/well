namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class UserThresholdService : IUserThresholdService
    {
        private readonly ICreditThresholdRepository creditThresholdRepository;
        private readonly IUserRepository userRepository;

        public UserThresholdService(ICreditThresholdRepository creditThresholdRepository,
            IUserRepository userRepository)
        {
            this.creditThresholdRepository = creditThresholdRepository;
            this.userRepository = userRepository;
        }

        public bool CanUserCredit(string username, int creditValue)
        {
            var user = this.userRepository.GetByIdentity(username);

            if (user == null) throw new ApplicationException($"User not found ({username})");

            var threshold = this.creditThresholdRepository.GetAll().FirstOrDefault(x => x.Id == user.ThresholdLevelId);

            if (threshold == null) throw new UserThresholdNotFoundException($"Threshold not found with id ({user.ThresholdLevelId})");

            if (creditValue <= threshold.Threshold) return true;
            
            return false;
        }

        public void AssignPendingCredit(CreditEvent creditEvent, string originator)
        {
            var branchSpecificThresholds = this.creditThresholdRepository.GetByBranch(creditEvent.BranchId);

            if (!this.ApplyThreshold(branchSpecificThresholds, ThresholdLevel.Level2, creditEvent, originator))
            {
                if (!this.ApplyThreshold(branchSpecificThresholds, ThresholdLevel.Level1, creditEvent, originator))
                {
                    throw new ApplicationException($"There are no levels that can handle the credit value of ({creditEvent.TotalCreditValueForThreshold}) for branch ({creditEvent.BranchId})");
                }
            }
        }

        private bool ApplyThreshold(IEnumerable<CreditThreshold> branchThresholds, ThresholdLevel level, CreditEvent creditEvent, string originator)
        {
            var threshold = branchThresholds.FirstOrDefault(x => x.ThresholdLevel == level);

            if (threshold == null)
                throw new ApplicationException($"Threshold not found for branch ({creditEvent.BranchId})");

            if (creditEvent.TotalCreditValueForThreshold <= threshold.Threshold)
            {
                var user = this.userRepository.GetUserByCreditThreshold(threshold);

                if (user == null) throw new ApplicationException("User not found for credit threshold");

                this.creditThresholdRepository.AssignPendingCreditToUser(user, creditEvent, originator);

                return true;
            }

            return false;
        }
    }
}