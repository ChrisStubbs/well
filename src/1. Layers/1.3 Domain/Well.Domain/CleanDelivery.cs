namespace PH.Well.Domain
{
    public class CleanDelivery:Entity<int>
    {
        public CleanDelivery()
        {
        }

        public string  RouteNumber { get; set; }
        public int DropNumber { get; set; }
        public int InvoiceNumber { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string JobStatus { get; set; }

    }
}
