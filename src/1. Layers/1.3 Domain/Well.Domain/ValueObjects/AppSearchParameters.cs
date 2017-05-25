namespace PH.Well.Domain.ValueObjects
{
    using System;

    public class AppSearchParameters
    {
        public int? BranchId { get; set; }
        public DateTime? Date { get; set; }
        public string Account { get; set; }
        public string Invoice { get; set; }
        public string Route { get; set; }
        public string Driver { get; set; }
        public int? DeliveryType { get; set; }
        public int? Status { get; set; }

    }
}