using PH.Well.Domain.Enums;

namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.Contracts;
    using Domain.ValueObjects;

    public interface IBulkEditService
    {
        PatchSummary GetByLineItems(IEnumerable<int> lineItemIds, DeliveryAction deliveryAction);
        PatchSummary GetByJobs(IEnumerable<int> jobIds, DeliveryAction deliveryAction);
        IEnumerable<Job> GetEditableJobsByLineItemId(IEnumerable<int> lineItemIds);
        IEnumerable<Job> GetEditableJobsByJobId(IEnumerable<int> jobId);
        BulkEditResult Update(IEnumerable<Job> editableJobs, ILineItemActionResolution resolution, IEnumerable<int> lineItemIds);

    }
}