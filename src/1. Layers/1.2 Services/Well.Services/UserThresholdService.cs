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
    using Repositories;

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

        public ThresholdResponse CanUserCredit(string username, decimal creditValue)
        {
            var response = new ThresholdResponse();
            var user = this.userRepository.GetByIdentity(username);

            if (user == null) 
            {
                response.IsInError = true;
                response.ErrorMessage = $"User not found ({username})";
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

        public void AssignPendingCredit(int branchId, decimal totalThresholdAmount, int jobId, string originator)
        {
            var branchSpecificThresholds = this.creditThresholdRepository.GetByBranch(branchId);

            if (!this.ApplyThreshold(branchSpecificThresholds, ThresholdLevel.Level2, branchId, totalThresholdAmount, originator, jobId))
            {
                if (!this.ApplyThreshold(branchSpecificThresholds, ThresholdLevel.Level1, branchId, totalThresholdAmount, originator, jobId))
                {
                    throw new ApplicationException(
                        $"There are no levels that can handle the credit value of ({totalThresholdAmount}) for branch ({branchId})");
                }
            }
        }

        private bool ApplyThreshold(IEnumerable<CreditThreshold> branchThresholds, ThresholdLevel level, int branchId, decimal totalThresholdAmount, string originator, int jobId)
        {
            var threshold = branchThresholds.FirstOrDefault(x => x.ThresholdLevel == level);

            if (threshold == null)
                throw new ApplicationException($"Threshold not found for branch ({branchId})");

            if (totalThresholdAmount <= threshold.Threshold)
            {
                creditThresholdRepository.PendingCreditInsert(jobId, originator);
                userRepository.UnAssignJobToUser(jobId);
                return true;
            }

            return false;
        }
    }
}