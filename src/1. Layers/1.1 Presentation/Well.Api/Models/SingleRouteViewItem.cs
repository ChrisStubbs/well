namespace PH.Well.Api.Models
{
    using Domain.Enums;

    public class SingleRouteItem
    {
        public int JobId { get; set; }
        public string Stop { get; set; }
        public string StopStatus { get; set; }
        public int StopExceptions { get; set; }
        public int StopClean { get; set; }
        public int Tba { get; set; }
        public string StopAssignee { get; set; }
        public string Resolution { get; set; }
        public string Invoice { get; set; }
        public string JobType { get; set; }
        public int JobTypeId { get; set; }
        public string Cod { get; set; }
        public bool Pod { get; set; }
        public int Exceptions { get; set; }
        public int InvoicedQty => Exceptions + Clean;
        public int Clean { get; set; }
        public decimal? Credit { get; set; }
        public string Assignee { get; set; }
        public string JobStatusDescription { get; set; }
        public JobStatus JobStatus { get; set; }
        public int StopId { get; set; }
        public string Account { get; set; }
        public WellStatus WellStatus { get; set; }
        public string WellStatusDescription { get; set; }
    }
}