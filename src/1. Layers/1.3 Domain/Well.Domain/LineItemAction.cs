namespace PH.Well.Domain
{
    using System;
    using System.Security.AccessControl;

    public class LineItemAction: Entity<int>
    {
        public int LineItemId { get; set; }
        public int ExceptionTypeId { get; set; }
        public int Quantity { get; set; }
        public int SourceId { get; set; }
        public int ReasonId { get; set; }
        public DateTime ReplanDate { get; set; }
        public DateTime SubmittedDate { get; set; } 
        public DateTime ApprovalDate { get; set; }
        public string ApprovedBy { get; set; }
    }
}
