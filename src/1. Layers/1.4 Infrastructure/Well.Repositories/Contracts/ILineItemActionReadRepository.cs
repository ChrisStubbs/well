namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface ILineItemActionReadRepository
    {
        IEnumerable<LineItemAction> GetLineItemActionByLineItemId(int id);
        IEnumerable<LineItemAction> GetLineItemActionByLineItemIds(IEnumerable<int> id);
    }
}
