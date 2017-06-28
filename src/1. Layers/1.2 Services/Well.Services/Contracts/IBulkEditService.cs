namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain.ValueObjects;

    public interface IBulkEditService
    {
        BulkEditSummary GetByLineItems(IEnumerable<int> lineItemIds);
        BulkEditSummary GetByJobs(IEnumerable<int> jobIds);
    }
}