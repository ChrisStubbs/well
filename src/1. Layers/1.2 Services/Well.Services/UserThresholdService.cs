namespace PH.Well.Services
{
    using System;
    using System.CodeDom;
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

        public virtual ThresholdResponse CanUserCredit(decimal creditValue)
        {
            var username = this.userNameProvider.GetUserName();
            var response = new ThresholdResponse();
            var userCreditThreshold = GetCreditThreshold(username);

            if (userCreditThreshold == null)
            {
                response.IsInError = true;
                response.ErrorMessage = $"You must be assigned a threshold level before crediting.";
                return response;
            }

            if (creditValue <= userCreditThreshold.Threshold) response.CanUserCredit = true;

            return response;
        }

        public string AssignPendingCredit(int branchId, decimal totalThresholdAmount, int jobId)
        {
            throw new NotImplementedException();

            // Possible implementation ?
            var threshold = GetCreditThreshold(userNameProvider.GetUserName());
            if (threshold != null && totalThresholdAmount <= threshold.Threshold)
            {
                creditThresholdRepository.PendingCreditInsert(jobId);
                userRepository.UnAssignJobToUser(jobId);

                return string.Empty;
            }

            return $"There are no levels that can handle the credit value of ({totalThresholdAmount}) for branch ({branchId})";

            //var branchSpecificThresholds = this.creditThresholdRepository.GetByBranch(branchId);

            //if (!this.ApplyThreshold(branchSpecificThresholds, ThresholdLevel.Level2, branchId, totalThresholdAmount, jobId))
            //{
            //    if (!this.ApplyThreshold(branchSpecificThresholds, ThresholdLevel.Level1, branchId, totalThresholdAmount, jobId))
            //    {
            //        return $"There are no levels that can handle the credit value of ({totalThresholdAmount}) for branch ({branchId})";
            //    }
            //}
            //return string.Empty;
        }

        public decimal GetUserCreditThresholdValue()
        {
            var threshold = GetCreditThreshold(userNameProvider.GetUserName());
            return threshold?.Threshold ?? 0;
        }

        public bool UserHasRequiredCreditThreshold(Job job)
        {
            if (job.GetAllLineItemActions().All(x => x.DeliveryAction != DeliveryAction.Credit))
            {
                return true;
            }
            var creditValue = job.LineItems.Sum(x => x.TotalCreditValue);
            return CanUserCredit(creditValue).CanUserCredit;
        }

        public CreditThreshold GetCreditThreshold(string userName)
        {
            var user = userRepository.GetByIdentity(userName);
            if (user.CreditThresholdId.HasValue)
            {
                return creditThresholdRepository.GetById(user.CreditThresholdId.Value);
            }
            return null;}

        public void SetThresholdLevel(string userName, int creditThresholdId)
        {
            var user = userRepository.GetByIdentity(userName);
            var threshold = creditThresholdRepository.GetById(creditThresholdId);
            user.CreditThresholdId = threshold.Id;
            userRepository.Update(user);
        }
    }
}