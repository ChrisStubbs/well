namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface IBulkEditSummaryMapper
    {
        BulkEditSummary Map(IEnumerable<Job> jobs);
        BulkEditSummary Map(IEnumerable<Job> jobs, IEnumerable<int> lineItemIds);
    }
}