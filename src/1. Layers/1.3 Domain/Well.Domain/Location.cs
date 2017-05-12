namespace PH.Well.Domain
{
    using System.Collections.Generic;

    public class Location : Entity<int>
    {
        public Location()
        {
            this.Activities = new List<Activity>();
        }

        public int BranchId { get; set; }
        public string AccountCode { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Postcode { get; set; }

        public List<Activity> Activities { get; set; }
    }
}
