namespace PH.Well.Api.Validators.Contracts
{
    using System.Collections.Generic;

    using PH.Well.Api.Models;

    public interface IWidgetWarningValidator
    {
        List<string> Errors { get; set; }

        bool IsValid(WidgetWarningModel model, bool isUpdate);
    }
}
