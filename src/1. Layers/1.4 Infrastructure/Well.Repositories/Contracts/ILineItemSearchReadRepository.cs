namespace PH.Well.Repositories.Contracts
{
    using System;
    using System.Collections.Generic;
    using Domain;
    using Domain.Enums;

    public interface ILineItemSearchReadRepository
    {
        LineItem GetById(int id);

        IEnumerable<LineItem> GetLineItemByIds(IEnumerable<int> ids);

        IEnumerable<LineItem> GetLineItemByActivityId(int id);

        IEnumerable<LineItem> GetLineItemByJobIds(IEnumerable<int> jobIds);

        IEnumerable<LineItem> GetLineItemBranchRouteDate(int branchId, DateTime routeDate);
    }
}
