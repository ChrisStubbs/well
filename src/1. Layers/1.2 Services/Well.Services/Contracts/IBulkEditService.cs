namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.Contracts;
    using Domain.ValueObjects;

    public interface IBulkEditService
    {
        PatchSummary GetByLineItems(IEnumerable<int> lineItemIds);
        PatchSummary GetByJobs(IEnumerable<int> jobIds);
        IEnumerable<Job> GetEditableJobsByLineItemId(IEnumerable<int> lineItemIds);
        IEnumerable<Job> GetEditableJobsByJobId(IEnumerable<int> jobId);
        BulkEditResult Update(IEnumerable<Job> editableJobs, ILineItemActionResolution resolution, IEnumerable<int> lineItemIds);

    }
}