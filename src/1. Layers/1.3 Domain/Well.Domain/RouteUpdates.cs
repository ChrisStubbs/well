namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using ValueObjects;

    [Serializable()]
    [XmlRoot("StopDeliveries")]
    public class RouteUpdates
    {
        public RouteUpdates()
        {
            this.Stops = new List<StopUpdate>();
        }

        [XmlArray("Orders")]
        [XmlArrayItem("Order", typeof(StopUpdate))]
        public List<StopUpdate> Stops { get; set; }
    }
}
