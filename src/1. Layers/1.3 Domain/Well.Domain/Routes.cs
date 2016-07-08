namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    [Serializable()]
    //[XmlRoot("Routes")]
    public class Routes: Entity<int>
    { 
        public Routes()
        {
            this.RouteHeaders = new Collection<RouteHeader>();
        }

        public string FileName { get; set; }

        public DateTime ImportDate { get; set; }

        //[XmlArray("RouteHeaders")]
        //[XmlArrayItem("RouteHeader", typeof(RouteHeader))]
        public Collection<RouteHeader> RouteHeaders { get; set; }
    }
}
