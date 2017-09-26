namespace PH.Well.Domain.Contracts
{
    public interface IStopMoveIdentifiers
    {
        string PlannedStopNumber { get; set; }
        int RouteHeaderId { get; set; }
    }
}