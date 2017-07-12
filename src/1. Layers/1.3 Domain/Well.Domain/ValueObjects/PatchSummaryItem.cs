namespace PH.Well.Domain.ValueObjects
{
    public class PatchSummaryItem
    {
        public int JobId { get; set; }
        public string Invoice { get; set; }
        public string @Type { get; set; }
        public string Account { get; set; }
        public int ShortQuantity { get; set; }
        public int DamageQuantity { get; set; }
        public int BypassQuantity { get; set; }
        public decimal TotalValue { get; set; }
    }
}