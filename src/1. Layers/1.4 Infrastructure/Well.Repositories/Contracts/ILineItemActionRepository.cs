namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;

    public interface ILineItemActionRepository : IRepository<LineItemAction, int>
    {
        LineItemAction GetById(int id);
        IList<LineItemAction> GetByIds(int[] ids);
    }
}
