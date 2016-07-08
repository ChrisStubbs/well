using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Domain
{
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    [Serializable()]
    [XmlRoot("RouteDeliveries")]
    public class RouteDeliveries
    {
        [XmlArray("Routes")]
        [XmlArrayItem("RouteHeader", typeof(RouteHeader))]
        public Collection<RouteHeader> RouteHeaders { get; set; }
    }
}
