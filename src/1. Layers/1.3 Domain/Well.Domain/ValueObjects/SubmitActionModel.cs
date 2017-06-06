namespace PH.Well.Domain.ValueObjects
{
    using Enums;

    public class SubmitActionModel
    {
        public DeliveryAction Action { get; set; }
        public int[] JobIds { get; set; }
    }
}