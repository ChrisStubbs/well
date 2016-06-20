namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class RouteImport: Entity<int>
    {
        public RouteImport()
        {
            this.RouteHeaders = new Collection<RouteHeader>();
        }

        public string FileName { get; set; }

        public DateTime ImportDate { get; set; }

        public Collection<RouteHeader> RouteHeaders { get; set; }
    }
}
