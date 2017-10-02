namespace PH.Well.Services.Contracts
{
    using System.Collections.Generic;
    using Domain;
    using Domain.ValueObjects;

    public interface IActionSummaryMapper
    {
        ActionSubmitSummary Map(bool isStopLevel, IList<Job> jobs);
    }
}