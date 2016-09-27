namespace PH.Well.Api.Validators
{
    using System.Collections.Generic;
    using System.Linq;

    using PH.Well.Api.Models;
    using PH.Well.Api.Validators.Contracts;
    using PH.Well.Domain.Enums;
    using PH.Well.Domain.Extensions;
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

        public bool IsValid(CleanPreferenceModel model)
        {
            if (!model.Days.HasValue)
            {
                this.Errors.Add("Days is required!");
            }

            this.ValidateAgainstExistingCleans(model);

            return !this.Errors.Any();
        }

        private void ValidateAgainstExistingCleans(CleanPreferenceModel model)
        {
            var existingCleans = this.cleanPreferenceRepository.GetAll();

            foreach (var clean in existingCleans)
            {
                var existingBranches = clean.Branches.Except(model.Branches);

                if (existingBranches.Any())
                {
                    this.Errors.Add("Clean preference has branches already assigned!");
                }
            }
        }
    }
}