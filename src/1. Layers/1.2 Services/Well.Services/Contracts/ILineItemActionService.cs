namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface ILineItemActionService
    {
        LineItemAction InsertLineItemActions(LineItemActionUpdate lineItemActionUpdate);
        LineItemAction UpdateLineItemActions(LineItemActionUpdate lineItemActionUpdate);
    }
}