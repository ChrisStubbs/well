namespace PH.Well.Api.Models
{
    public class DamageModel
    {
        public int Quantity { get; set; }

        public int JobDetailSourceId { get; set; }

        public int JobDetailReasonId { get; set; }

        public int DamageActionId { get; set; }

        public string JobDetailSource { get; set; }

        public string JobDetailReason { get; set; }

        public string DamageAction { get; set; }
    }
}