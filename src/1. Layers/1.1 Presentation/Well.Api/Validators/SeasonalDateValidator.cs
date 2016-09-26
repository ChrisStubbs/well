namespace PH.Well.Api.Validators
{
    using System;
    using System.Collections.Generic;

    using PH.Well.Api.Models;
    using PH.Well.Api.Validators.Contracts;

    public class SeasonalDateValidator : ISeasonalDateValidator
    {
        public SeasonalDateValidator()
        {
            this.Errors = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Errors { get; set; }

        public bool Isvalid(SeasonalDateModel model)
        {
            DateTime fromDate;

            if (!DateTime.TryParse(model.FromDate, out fromDate))
            {
                this.Errors.Add("FromDate", "From date is not a valid date!");
            }

            DateTime toDate;

            if (!DateTime.TryParse(model.ToDate, out toDate))
            {
                this.Errors.Add("ToDate", "To date is not a valid date!");
            }

            if (model.Description.Length > 255)
            {
                this.Errors.Add("Description", "Is over the max capacity of 255 characters!");
            }

            return this.Errors.Count == 0;
        }
    }
}
