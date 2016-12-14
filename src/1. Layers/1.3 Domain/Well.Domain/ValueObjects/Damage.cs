namespace PH.Well.Domain.ValueObjects
{
    using Enums;

    public class Damage
    {
        public int JobDetailId { get; set; }

        public int Quantity { get; set; }

        public int JobDetailSource { get; set; }

        public int JobDetailReason { get; set; }

        public JobDetailReason Reason { get; set; }
    }
}
