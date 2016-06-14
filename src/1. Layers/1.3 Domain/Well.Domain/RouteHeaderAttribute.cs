namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using PH.Well.Domain.Base;

    public class RouteHeaderAttribute : AttributeBase
    {
        public int RouteHeaderId { get; set; }
    }
}
