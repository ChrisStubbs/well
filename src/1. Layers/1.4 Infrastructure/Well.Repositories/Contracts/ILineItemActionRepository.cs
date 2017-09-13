namespace PH.Well.Repositories.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;

    public interface ILineItemActionRepository : IRepository<LineItemAction, int>
    {
        LineItemAction GetById(int id);
        IList<LineItemAction> GetByIds(IEnumerable<int> ids);
        IList<LineItemActionSubmitModel> GetUnsubmittedActions(DeliveryAction submitActionAction);
        void DeleteAllLineItemActionsForJob(int jobId);
        void Delete(LineItemAction entity);
    }
}
