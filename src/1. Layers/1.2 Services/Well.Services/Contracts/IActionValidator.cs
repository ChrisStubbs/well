namespace PH.Well.Services.Contracts
{
    using Domain.ValueObjects;

    public interface ISubmitCreditActionValidator : IActionValidator
    {
        
    }

    public interface IActionNotDefinedValidator: IActionValidator
    {

    }

    public interface IActionValidator
    {
        SubmitActionResult ValidateAction(SubmitActionModel action);
    }
}