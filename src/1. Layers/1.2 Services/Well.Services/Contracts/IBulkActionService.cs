namespace PH.Well.Services.Contracts
{
    using Domain.Enums;
    using Domain.ValueObjects;

    public interface IBulkActionService
    {
        BulkActionResults ApplyAction(BulkActionModel bulkAction);
        IActionValidator GetValidator(DeliveryAction deliveryAction);
    }
}