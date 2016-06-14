namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using PH.Well.Domain.Base;

    public class RoueImport: Entity<int>
    {
        public RoueImport()
        {
            this.RouteHeaders = new List<RouteHeader>();
        }

        public string FileName { get; set; }
        public DateTime ImportDate { get; set; }
        public List<RouteHeader> RouteHeaders { get; set; }
    }
}
