namespace PH.Well.Domain
{
    using System;

    public class JobResolutionStatus 
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public int JobId { get; set; }
        public string By { get; set; }
        public DateTime On { get; set; }
    }
}