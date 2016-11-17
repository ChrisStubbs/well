namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;

    [Serializable()]
    [XmlRoot("Orders")]
    public class Orders
    {
        [XmlArray("Orders")]
        [XmlArrayItem("Order", typeof(Order))]
        public Collection<Order> Order { get; set; }
    }
}
