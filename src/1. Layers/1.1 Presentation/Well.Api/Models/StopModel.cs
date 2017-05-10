namespace PH.Well.Api.Models
{
    using System;
    using System.Collections.Generic;

    public class StopModel
    {
        public StopModel()
        {
            Items = new List<StopModelItem>();
        }
        public int RouteId { get; set; }
        public string RouteNumber { get; set; }
        public string Branch { get; set; }
        public int BranchId { get; set; }
        public string Driver { get; set; }
        public DateTime? RouteDate { get; set; }
        public string AssignedTo { get; set; }
        public int Tba { get; set; }
        public string StopNo { get; set; }
        public int TotalNoOfStops { get; set; }
        public IList<StopModelItem> Items { get; set; }
    }
}
