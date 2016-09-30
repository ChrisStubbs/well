namespace PH.Well.Api.Validators
{
    using System.Collections.Generic;
    using System.Linq;

    using PH.Well.Api.Models;
    using PH.Well.Api.Validators.Contracts;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.Extensions;
    using PH.Well.Repositories.Contracts;

    public class CreditThresholdValidator : ICreditThresholdValidator
    {
        private readonly ICreditThresholdRepository creditThresholdRepository;

        public CreditThresholdValidator(ICreditThresholdRepository creditThresholdRepository)
        {
            this.creditThresholdRepository = creditThresholdRepository;
            this.Errors = new List<string>();
        }

        public List<string> Errors { get; set; }

        public bool IsValid(CreditThresholdModel model, bool isUpdate)
        {
            if (model.ThresholdLevel == "Level" && !isUpdate)
            {
                this.Errors.Add("Threshold level is required!");
            }
            else if (!isUpdate)
            {
                this.ValidateAgainstExistingThresholds(model);
            }

            if (!model.Threshold.HasValue)
            {
                this.Errors.Add("Threshold is required!");
            }
            else if (model.Threshold < 1 || model.Threshold > 1000000)
            {
                this.Errors.Add("Threshold range is 1 to 1000000");
            }

            if (!model.Branches.Any())
            {
                this.Errors.Add("Branch is required!");
            }

            return !this.Errors.Any();
        }

        private void ValidateAgainstExistingThresholds(CreditThresholdModel model)
        {
            var existingThresholds = this.creditThresholdRepository.GetAll();

            var thresholdId = (int)EnumExtensions.GetValueFromDescription<ThresholdLevel>(model.ThresholdLevel);

            var thresholdsToCheck = existingThresholds.Where(x => x.ThresholdLevelId == thresholdId);

            var branchAlreadyHasAThreshold = false;

            foreach (var threshold in thresholdsToCheck)
            {
                foreach (var branch in threshold.Branches)
                {
                    var modelBranch = model.Branches.FirstOrDefault(x => x.Id == branch.Id);

                    if (modelBranch != null)
                    {
                        branchAlreadyHasAThreshold = true;
                        break;
                    }
                }

                if (branchAlreadyHasAThreshold)
                {
                    this.Errors.Add("Branches already have a threshold assigned!");
                    break;
                }
            }
        }
    }
}