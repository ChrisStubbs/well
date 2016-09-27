namespace PH.Well.Api.Validators.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Api.Models;

    public interface ICreditThresholdValidator
    {
        List<string> Errors { get; set; }

        bool IsValid(CreditThresholdModel model);
    }
}
