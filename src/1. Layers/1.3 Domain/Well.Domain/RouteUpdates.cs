namespace PH.Well.Domain
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using ValueObjects;

    [Serializable()]
    [XmlRoot("RouteUpdates")]
    public class RouteUpdates
    {
        [XmlArray("Orders")]
        [XmlArrayItem("Order", typeof(Order))]
        public Collection<Order> Order { get; set; }
    }
}
