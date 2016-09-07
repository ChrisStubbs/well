namespace PH.Well.Domain.ValueObjects
{
    using Enums;

    public class Damage
    {
        public int JobDetailId { get; set; }

        public int Quantity { get; set; }

        public DamageReasons Reason { get; set; }
    }
}
