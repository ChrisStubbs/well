namespace PH.Well.Domain
{
    using System;
    using System.Security.AccessControl;
    using Enums;

    public class LineItemAction: Entity<int>
    {
        public int LineItemId { get; set; }
        public string ExceptionType { get; set; }
        public int Quantity { get; set; }
        public string Source { get; set; }
        public string Reason { get; set; }
        public DateTime? ReplanDate { get; set; }
        public DateTime? SubmittedDate { get; set; } 
        public DateTime? ApprovalDate { get; set; }
        public string ApprovedBy { get; set; }
        public string ActionedBy { get; set; }
        public string Originator { get; set; }

    }
}
