using System;
using System.Collections.Generic;

namespace PH.Well.Api.Models
{
    public class SingleRouteView
    {
        public SingleRouteView()
        {
            Items = new List<SingleRouteViewItem>();
        }
        public int Id { get; set; }
        public string RouteNumber { get; set; }
        public string Branch { get; set; }
        public string Driver { get; set; }
        public DateTime? RouteDate { get; set; }
        public IList<SingleRouteViewItem> Items { get; set; }
    }
}