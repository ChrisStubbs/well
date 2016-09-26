namespace PH.Well.Api.Validators.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Api.Models;

    public interface ISeasonalDateValidator
    {
        Dictionary<string, string> Errors { get; set; }

        bool Isvalid(SeasonalDateModel model);
    }
}
