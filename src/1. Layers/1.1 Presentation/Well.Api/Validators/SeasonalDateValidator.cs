namespace PH.Well.Api.Validators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using PH.Well.Api.Models;
    using PH.Well.Api.Validators.Contracts;

    public class SeasonalDateValidator : ISeasonalDateValidator
    {
        public SeasonalDateValidator()
        {
            this.Errors = new List<string>();
        }

        public List<string> Errors { get; set; }

        public bool IsValid(SeasonalDateModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Description))
            {
                this.Errors.Add("Description is required!");
            }
            else if (model.Description.Length > 255)
            {
                this.Errors.Add("Description is over the max capacity of 255 characters!");
            }

            bool anyDateError = false;
            DateTime fromDate = new DateTime();

            if (string.IsNullOrWhiteSpace(model.FromDate))
            {
                this.Errors.Add("From date is required!");
                anyDateError = true;
            }
            else if (!DateTime.TryParse(model.FromDate, out fromDate))
            {
                this.Errors.Add("From date is not a valid date!");
                anyDateError = true;
            }

            DateTime toDate = new DateTime();

            if (string.IsNullOrWhiteSpace(model.ToDate))
            {
                this.Errors.Add("To date is required!");
                anyDateError = true;
            }
            else if (!DateTime.TryParse(model.ToDate, out toDate))
            {
                this.Errors.Add("To date is not a valid date!");
                anyDateError = true;
            }

            if (!anyDateError && fromDate.Date > toDate.Date)
            {
                this.Errors.Add("From date can not be greater than to date!");
            }

            if (model.Branches.Count == 0)
            {
                this.Errors.Add("Select a branch!");
            }

            return !this.Errors.Any();
        }
    }
}
