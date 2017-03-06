namespace PH.Well.Services.Contracts
{
    using Domain;
    using Domain.Enums;
    using Domain.ValueObjects;

    public interface IDeliveryLinesAction
    {
        ProcessDeliveryActionResult Execute(Job job);
        DeliveryAction Action { get; }
    }
}
