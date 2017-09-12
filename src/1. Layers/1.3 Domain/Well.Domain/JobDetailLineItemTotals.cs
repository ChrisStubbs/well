namespace PH.Well.Domain
{
    public class JobDetailLineItemTotals
    {
        public int BypassTotal { get; set; }
        public int DamageTotal { get; set; }
        public int ShortTotal { get; set; }
        public int TotalExceptions { get; set; }
        public int JobDetailId { get; set; }
        public int? StopId { get; set; }
        public int? RouteId { get; set; }
        public int? JobId { get; set; }
    }
}
