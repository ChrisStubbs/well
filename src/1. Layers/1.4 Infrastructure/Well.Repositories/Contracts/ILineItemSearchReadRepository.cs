namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface ILineItemSearchReadRepository
    {
        IEnumerable<LineItem> GetLineItemByIds(IEnumerable<int> ids);
        IEnumerable<LineItem> GetLineItemByActivityId(int id);
    }
}
