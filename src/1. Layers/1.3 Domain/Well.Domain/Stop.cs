namespace PH.Well.Domain
{
    using System;
    using System.Collections.ObjectModel;
 
    public class Stop:Entity<int>
    {
        public Stop()
        {
            this.Accounts = new Collection<Account>();
            this.Stops = new Collection<Stop>();
        }

        public string PLannedStopNumber { get; set; }

        public TimeSpan PLannedArrivalTime { get; set; }

        public TimeSpan PLannedDepartTime { get; set; }

        public int RouteHeaderId { get; set; }

        public int DropId { get; set; }

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

        public Collection<Stop> Stops { get; set; }
    }
}
