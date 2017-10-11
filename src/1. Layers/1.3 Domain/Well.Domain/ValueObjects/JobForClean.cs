namespace PH.Well.Domain.ValueObjects
{
    using System;
    using Enums;

    public class JobForClean
    {
        public int JobId { get; set; }
        public int? JobRoyaltyCodeId { get; set; }
        public string JobRoyaltyCode { get; set; }
        public int StopId { get; set; }
        public int RouteId { get; set; }
        public DateTime RouteDate { get; set; }
        public ResolutionStatus ResolutionStatusId { get; set; }
        public int BranchId { get; set; }
    }
}