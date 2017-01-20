namespace PH.Well.Domain
{
    public class Notification : Entity<int>
    {
        public int JobId { get; set; }

        public int Type { get; set; }

        public string Reason { get; set; }

        public string Source { get; set; }

        public bool IsArchived { get; set; }

        public string PhAccount { get; set; } 

        public string PicklistRef { get; set; }

        public string InvoiceNumber { get; set; }

        public string ContactName { get; set; }

        public string DepotId { get; set; }

        public string UserName { get; set; }

        public string ErrorMessage { get; set; }


    }
}
