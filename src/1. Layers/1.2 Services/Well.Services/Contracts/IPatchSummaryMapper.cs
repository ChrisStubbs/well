namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface IPatchSummaryMapper
    {
        PatchSummary Map(IEnumerable<Job> jobs);
        PatchSummary Map(IEnumerable<Job> jobs, IEnumerable<int> lineItemIds);
    }
}