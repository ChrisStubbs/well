namespace PH.Well.Domain.ValueObjects
{
    using System;

    public class JobRoute
    {
        public int JobId { get; set; }
        public int RouteId { get; set; }
        public DateTime RouteDate { get; set; }
        public int BranchId { get; set; }
    }
}