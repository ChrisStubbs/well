namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface ILineItemActionService
    {
        LineItem InsertLineItemActions(LineItemActionUpdate lineItemActionUpdate);
        LineItem UpdateLineItemActions(LineItemActionUpdate lineItemActionUpdate);
        LineItem SaveLineItemActions(Job job, int lineItemId, IEnumerable<LineItemAction> lineItemActions);
    }
}