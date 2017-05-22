namespace PH.Well.Services.Contracts
{
    using Domain.ValueObjects;

    public interface IBulkActionService
    {
        BulkActionResults ApplyAction(BulkActionModel bulkAction);
    }
}