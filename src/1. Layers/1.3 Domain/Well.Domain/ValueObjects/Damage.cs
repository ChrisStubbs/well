namespace PH.Well.Domain.ValueObjects
{
    public class Damage
    {
        public int JobDetailId { get; set; }

        public int Quantity { get; set; }

        public int JobDetailSourceId { get; set; }

        public int JobDetailReasonId { get; set; }

        public int DamageActionId { get; set; }
    }
}
