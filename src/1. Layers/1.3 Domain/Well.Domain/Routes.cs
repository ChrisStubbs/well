namespace PH.Well.Domain
{
    using System;
    using System.Collections.ObjectModel;
    [Serializable()]
    public class Routes: Entity<int>
    { 
        public Routes()
        {
            this.RouteHeaders = new Collection<RouteHeader>();
        }

        public string FileName { get; set; }

        public DateTime ImportDate { get; set; }

        public Collection<RouteHeader> RouteHeaders { get; set; }
    }
}
