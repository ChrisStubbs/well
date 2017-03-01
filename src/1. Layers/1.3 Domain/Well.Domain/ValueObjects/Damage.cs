namespace PH.Well.Domain.ValueObjects
{
    using Enums;

    public class Damage
    {
        public int JobDetailId { get; set; }

        public int Quantity { get; set; }

        public int JobDetailSourceId { get; set; }

        public int JobDetailReasonId { get; set; }

        public int DamageActionId { get; set; }

        public DeliveryAction DamageAction => (DeliveryAction) DamageActionId;
    }
}
