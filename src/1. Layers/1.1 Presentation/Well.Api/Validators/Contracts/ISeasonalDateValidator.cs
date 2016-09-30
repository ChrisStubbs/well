namespace PH.Well.Api.Validators.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Api.Models;

    public interface ISeasonalDateValidator
    {
        List<string> Errors { get; set; }

        bool IsValid(SeasonalDateModel model);
    }
}
