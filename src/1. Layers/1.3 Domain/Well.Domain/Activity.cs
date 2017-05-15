namespace PH.Well.Domain
{
    using System.Collections.Generic;

    public class Activity : Entity<int>
    {
        public Activity()
        {
            this.LineItems = new List<LineItem>();
        }

        public string DocumentNumber { get; set; }
        public string InitialDocument { get; set; }
        public int ActivityTypeId { get; set; }

        public List<LineItem> LineItems { get; set; }
    }
}
