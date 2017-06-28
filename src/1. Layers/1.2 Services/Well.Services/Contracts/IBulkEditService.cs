namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.Contracts;
    using Domain.ValueObjects;

    public interface IBulkEditService
    {
        BulkEditSummary GetByLineItems(IEnumerable<int> lineItemIds);
        BulkEditSummary GetByJobs(IEnumerable<int> jobIds);
        IEnumerable<Job> GetEditableJobsByLineItemId(IEnumerable<int> lineItemIds);
        IEnumerable<Job> GetEditableJobsByJobId(IEnumerable<int> jobId);
        BulkEditResult Update(IEnumerable<Job> editableJobs, ILineItemActionResolution resolution, IEnumerable<int> lineItemIds);

    }
}