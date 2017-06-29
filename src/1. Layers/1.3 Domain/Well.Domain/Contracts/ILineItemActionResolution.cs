namespace PH.Well.Domain.Contracts
{
    using Enums;
    public interface ILineItemActionResolution
    {
        DeliveryAction DeliveryAction { get; set; }
        JobDetailSource Source { get; set; }
        JobDetailReason Reason { get; set; }
    }
}