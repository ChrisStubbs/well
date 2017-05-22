namespace PH.Well.Api.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Domain.Enums;
    using Models;
    using Repositories.Contracts;

    public class WidgetWarningValidator : IWidgetWarningValidator
    {
        private readonly IWidgetRepository widgetRepository;

        public WidgetWarningValidator(IWidgetRepository widgetRepository)
        {
            this.widgetRepository = widgetRepository;
            this.Errors = new List<string>();
        }

        public List<string> Errors { get; set; }

        public bool IsValid(WidgetWarningModel model, bool isUpdate)
        {
            if (!model.WarningLevel.HasValue)
            {
                this.Errors.Add("Warning level is required");
            }
            else if (model.WarningLevel < 1 || model.WarningLevel > 200)
            {
               this.Errors.Add("Range is 1 to 200");
            }

            if (!model.Branches.Any() && !isUpdate)
            {
                this.Errors.Add("Branch is required!");
            }

            if (!isUpdate) this.ValidateAgainstExistingWarnings(model);

            return !this.Errors.Any();
        }

        private void ValidateAgainstExistingWarnings(WidgetWarningModel model)
        {
            var existingWarnings = this.widgetRepository.GetAll();

            var branchAlreadyHasAWarning = false;

            foreach (var warning in existingWarnings)
            {
                if (model.Type != warning.WidgetType.ToString())
                {
                    break;
                }

                foreach (var branch in warning.Branches)
                {
                    var modelBranch = model.Branches.FirstOrDefault(x => x.Id == branch.Id);
                    
                    if (modelBranch != null)
                    {
                        branchAlreadyHasAWarning = true;
                        break;
                    }
                }
            }

            if (branchAlreadyHasAWarning)
            {
                this.Errors.Add("Branch already has a warning assigned!");
            }
        }
    }
}