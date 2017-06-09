namespace PH.Well.Services.Contracts
{
    using Domain.ValueObjects;

    public interface IActionSummaryMapper
    {
        ActionSubmitSummary Map(SubmitActionModel submitAction, bool isStopLevel);
    }
}