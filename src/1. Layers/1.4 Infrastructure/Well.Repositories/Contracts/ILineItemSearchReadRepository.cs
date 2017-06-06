namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.Enums;

    public interface ILineItemSearchReadRepository
    {
        LineItem GetById(int id);
        IEnumerable<LineItem> GetLineItemByIds(IEnumerable<int> ids);
        IEnumerable<LineItem> GetLineItemByActivityId(int id);
    }
}
