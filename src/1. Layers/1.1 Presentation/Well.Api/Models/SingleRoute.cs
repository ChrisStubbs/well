using System;
using System.Collections.Generic;

namespace PH.Well.Api.Models
{
    public class SingleRoute
    {
        public SingleRoute()
        {
            Items = new List<SingleRouteItem>();
        }
        public int Id { get; set; }

        public string RouteNumber { get; set; }

        public string Branch { get; set; }

        public int BranchId { get; set; }

        public string Driver { get; set; }

        public DateTime? RouteDate { get; set; }

        public IList<SingleRouteItem> Items { get; set; }

    }
}