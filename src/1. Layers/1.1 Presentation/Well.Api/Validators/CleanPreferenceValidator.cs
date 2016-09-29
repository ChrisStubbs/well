namespace PH.Well.Api.Validators
{
    using System.Collections.Generic;
    using System.Linq;

    using PH.Well.Api.Models;
    using PH.Well.Api.Validators.Contracts;
    using PH.Well.Repositories.Contracts;

    public class CleanPreferenceValidator : ICleanPreferenceValidator
    {
        private readonly ICleanPreferenceRepository cleanPreferenceRepository;

        public CleanPreferenceValidator(ICleanPreferenceRepository cleanPreferenceRepository)
        {
            this.cleanPreferenceRepository = cleanPreferenceRepository;
            this.Errors = new List<string>();
        }

        public List<string> Errors { get; set; }

        public bool IsValid(CleanPreferenceModel model, bool isUpdate)
        {
            if (!model.Days.HasValue)
            {
                this.Errors.Add("Days is required!");
            }
            else if (model.Days < 1 || model.Days > 100)
            {
                this.Errors.Add("Days range is 1 to 100!");
            }

            if (!model.Branches.Any() && !isUpdate)
            {
                this.Errors.Add("Branch is required!");
            }

            if (!isUpdate) this.ValidateAgainstExistingCleans(model);

            return !this.Errors.Any();
        }

        private void ValidateAgainstExistingCleans(CleanPreferenceModel model)
        {
            var existingCleans = this.cleanPreferenceRepository.GetAll();

            var branchAlreadyHasACleanPreference = false;

            foreach (var clean in existingCleans)
            {
                foreach (var branch in clean.Branches)
                {
                    var modelBranch = model.Branches.FirstOrDefault(x => x.Id == branch.Id);

                    if (modelBranch != null)
                    {
                        branchAlreadyHasACleanPreference = true;
                        break;
                    }
                }
            }

            if (branchAlreadyHasACleanPreference)
            {
                this.Errors.Add("Branch already has a clean preference assigned!");
            }
        }
    }
}