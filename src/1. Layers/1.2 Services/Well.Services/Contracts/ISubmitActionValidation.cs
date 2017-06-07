namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;

    public interface ISubmitActionValidation
    {
        SubmitActionResult Validate(SubmitActionModel submitAction, IEnumerable<LineItemActionSubmitModel> allUnsubmittedItems);
    }

    public interface ISubmitCreditActionValidation : ISubmitActionValidation
    {
    }
}