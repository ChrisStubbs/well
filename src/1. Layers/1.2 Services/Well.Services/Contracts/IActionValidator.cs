namespace PH.Well.Services.Contracts
{
    using Domain.ValueObjects;

    public interface ICreditActionValidator : IActionValidator
    {
        
    }

    public interface IActionValidator
    {
        BulkActionResults ValidateAction(BulkActionModel bulkAction);
        BulkActionResults ValidateItems(BulkActionModel bulkAction);
    }
}