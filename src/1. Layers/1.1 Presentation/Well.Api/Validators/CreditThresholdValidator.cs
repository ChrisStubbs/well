namespace PH.Well.Api.Validators
{
    using System.Collections.Generic;
    using System.Linq;

    using PH.Well.Api.Models;
    using PH.Well.Api.Validators.Contracts;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.Extensions;
    using PH.Well.Repositories.Contracts;

    using WebGrease.Css.Extensions;

    public class CreditThresholdValidator : ICreditThresholdValidator
    {
        private readonly ICreditThresholdRepository creditThresholdRepository;

        public CreditThresholdValidator(ICreditThresholdRepository creditThresholdRepository)
        {
            this.creditThresholdRepository = creditThresholdRepository;
            this.Errors = new List<string>();
        }

        public List<string> Errors { get; set; }

        public bool IsValid(CreditThresholdModel model)
        {
            if (model.ThresholdLevel == "Level")
            {
                this.Errors.Add("Threshold level is required!");
            }

            if (!model.Threshold.HasValue)
            {
                this.Errors.Add("Threshold amount is required and should be a number only!");
            }

            if (model.Branches.Count == 0)
            {
                this.Errors.Add("Select a branch!");
            }

            this.ValidateAgainstExistingThresholds(model);

            return !this.Errors.Any();
        }

        private void ValidateAgainstExistingThresholds(CreditThresholdModel model)
        {
            var existingThresholds = this.creditThresholdRepository.GetAll();

            var thresholdId = (int)EnumExtensions.GetValueFromDescription<ThresholdLevel>(model.ThresholdLevel);

            var thresholdsToCheck = existingThresholds.Where(x => x.ThresholdLevelId == thresholdId);

            foreach (var threshold in thresholdsToCheck)
            {
                var existingBranches = threshold.Branches.Except(model.Branches);

                if (existingBranches.Any())
                {
                    this.Errors.Add("Threshold has branches already assigned!");
                    continue;
                }
            }
        }
    }
}