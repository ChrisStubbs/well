namespace PH.Well.Domain
{
    public class Notification : Entity<int>
    {
        public int JobId { get; set; }

        public int Type { get; set; }

        public string Reason { get; set; }

        public string Source { get; set; }

        public bool IsArchived { get; set; }

        public string JobRef1 { get; set; } 

        public string JobRef2 { get; set; }

        public string JobRef3 { get; set; }

        public string ContactName { get; set; }

        public string DepotId { get; set; }

    }
}
