namespace PH.Well.Services
{
    using System;
    using System.Linq;

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

            if (threshold == null) throw new ApplicationException($"Threshold not found with id ({user.ThresholdLevelId})");

            if (creditValue <= threshold.Threshold) return true;
            
            return false;
        }

        public void AssignPendingCredit(CreditEvent creditEvent, string originator)
        {
        }
    }
}