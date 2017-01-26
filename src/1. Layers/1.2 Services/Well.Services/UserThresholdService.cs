namespace PH.Well.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PH.Well.Common.Contracts;
    using PH.Well.Domain;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.ValueObjects;
    using PH.Well.Repositories.Contracts;
    using PH.Well.Services.Contracts;

    public class UserThresholdService : IUserThresholdService
    {
        private readonly ICreditThresholdRepository creditThresholdRepository;

        private readonly IUserRepository userRepository;

        private readonly ILogger logger;

        public UserThresholdService(
            ICreditThresholdRepository creditThresholdRepository,
            IUserRepository userRepository,
            ILogger logger)
        {
            this.creditThresholdRepository = creditThresholdRepository;
            this.userRepository = userRepository;
            this.logger = logger;
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

        /*public Dictionary<int, string> RemoveCreditEventsThatDontHaveAThreshold(List<CreditEvent> creditEvents, string originator)
        {
            var errors = new Dictionary<int, string>();

            var creditEventsToRemove = new List<CreditEvent>();

            foreach (var creditEvent in creditEvents)
            {
                try
                {
                    if (!this.CanUserCredit(originator, creditEvent.TotalCreditValueForThreshold))
                    {
                        try
                        {
                            //todo
                            //this.AssignPendingCredit(creditEvent, originator);
                            creditEventsToRemove.Add(creditEvent);
                        }
                        catch (ApplicationException exception)
                        {
                            this.logger.LogError("Error occurred when assigning credit to threshold!", exception);
                            errors.Add(creditEvent.Id, "Error occurred when assigning credit to threshold!");
                        }
                    }
                }
                catch (UserThresholdNotFoundException)
                {
                    errors.Add(creditEvent.Id, $"The threshold level does not exist for branch {creditEvent.BranchId}, invoice {creditEvent.InvoiceNumber}");
                    creditEventsToRemove.Add(creditEvent);
                }
            }

            foreach (var creditToRemove in creditEventsToRemove)
            {
                creditEvents.Remove(creditToRemove);
            }

            return errors;
        }*/

        private bool ApplyThreshold(IEnumerable<CreditThreshold> branchThresholds, ThresholdLevel level, int branchId, decimal totalThresholdAmount, string originator, int jobId)
        {
            var threshold = branchThresholds.FirstOrDefault(x => x.ThresholdLevel == level);

            if (threshold == null)
                throw new ApplicationException($"Threshold not found for branch ({branchId})");

            if (totalThresholdAmount <= threshold.Threshold)
            {
                var user = this.userRepository.GetUserByCreditThreshold(threshold);

                if (user == null) throw new ApplicationException("User not found for credit threshold");

                this.creditThresholdRepository.AssignPendingCreditToUser(user, jobId, originator);

                return true;
            }

            return false;
        }
    }
}