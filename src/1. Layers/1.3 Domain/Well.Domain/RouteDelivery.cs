﻿namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [Serializable()]
    [XmlRoot("RouteDeliveries")]
    public class RouteDelivery
    {
        public RouteDelivery()
        {
            this.RouteHeaders = new List<RouteHeader>();
        }

        public int RouteId { get; set; }

        [XmlArray("Routes")]
        [XmlArrayItem("RouteHeader", typeof(RouteHeader))]
        public List<RouteHeader> RouteHeaders { get; set; }
    }
}
