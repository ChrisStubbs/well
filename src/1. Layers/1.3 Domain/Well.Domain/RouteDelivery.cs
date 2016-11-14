namespace PH.Well.Domain
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    [Serializable()]
    [XmlRoot("RouteDeliveries")]
    public class RouteDelivery
    {
        [XmlArray("Routes")]
        [XmlArrayItem("RouteHeader", typeof(RouteHeader))]
        public Collection<RouteHeader> RouteHeaders { get; set; }
    }
}
