namespace PH.Well.Api.Validators.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Api.Models;

    public interface ICleanPreferenceValidator
    {
        List<string> Errors { get; set; }

        bool IsValid(CleanPreferenceModel model, bool isUpdate);
    }
}
