namespace PH.Well.Domain
{
    using System;
    using System.Collections.ObjectModel;

    public class Stop : Entity<int>
    {
        public Stop()
        {
            this.Accounts = new Collection<Account>();
        }

        public string PlannedStopNumber { get; set; }

        public TimeSpan PlannedArrivalTime { get; set; }

        public TimeSpan PlannedDepartTime { get; set; }

        public int RouteHeaderId { get; set; }

        public string DropId { get; set; }

        public int LocationId { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string SpecialInstructions { get; set; }

        public TimeSpan StartWindow { get; set; }

        public TimeSpan EndWindow { get; set; }

        public string TextField1 { get; set; }

        public string TextField2 { get; set; }

        public string TextField3 { get; set; }

        public string TextField4 { get; set; }

        public Collection<Account> Accounts { get; set; }
    }
}