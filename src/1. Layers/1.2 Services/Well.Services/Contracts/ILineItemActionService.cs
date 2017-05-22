namespace PH.Well.Services.Contracts
{
    using Domain;
    using Domain.ValueObjects;

    public interface ILineItemActionService
    {
        LineItem InsertLineItemActions(LineItemActionUpdate lineItemActionUpdate);
        LineItem UpdateLineItemActions(LineItemActionUpdate lineItemActionUpdate);
    }
}