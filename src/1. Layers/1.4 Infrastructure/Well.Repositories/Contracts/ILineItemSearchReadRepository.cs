namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface ILineItemSearchReadRepository
    {
        LineItem GetById(int id);
        IEnumerable<LineItem> GetLineItemByIds(IEnumerable<int> ids);
        IEnumerable<LineItem> GetLineItemByActivityId(int id);
    }
}
