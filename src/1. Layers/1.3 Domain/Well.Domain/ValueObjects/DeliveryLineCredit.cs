namespace PH.Well.Domain.ValueObjects
{
    public class DeliveryLineCredit
    {
        public int JobId { get; set; }

        public int Reason { get; set; }

        public int Source { get; set; }

        public int Quantity { get; set; }

        public string ProductCode { get; set; }
    }
}