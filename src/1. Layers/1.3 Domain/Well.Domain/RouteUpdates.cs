namespace PH.Well.Domain
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using ValueObjects;

    [Serializable()]
    [XmlRoot("StopDeliveries")]
    public class RouteUpdates
    {
        [XmlArray("Orders")]
        [XmlArrayItem("Order", typeof(StopUpdate))]
        public Collection<StopUpdate> Stops { get; set; }
    }
}
