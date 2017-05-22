namespace PH.Well.Domain.ValueObjects
{
    using Enums;

    public class BulkActionModel
    {
        public DeliveryAction Action { get; set; }
        public int[] JobDetailActionIds { get; set; }
    }
}