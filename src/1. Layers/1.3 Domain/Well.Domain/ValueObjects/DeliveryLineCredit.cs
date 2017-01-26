namespace PH.Well.Domain.ValueObjects
{
    using PH.Well.Domain.Enums;

    public class DeliveryLineCredit
    {
        public JobDetailReason Reason { get; set; }

        public JobDetailSource Source { get; set; }

        public int Quantity { get; set; }

        public string ProductCode { get; set; }
    }
}