namespace PH.Well.Domain
{
    using System;
    using Enums;

    public class LineItemAction: Entity<int>
    {
        public int LineItemId { get; set; }
        public ExceptionType ExceptionType { get; set; }
        public int Quantity { get; set; }
        public JobDetailSource Source { get; set; }
        public JobDetailReason Reason { get; set; }
        public DateTime? ReplanDate { get; set; }
        public DateTime? SubmittedDate { get; set; } 
        public DateTime? ApprovalDate { get; set; }
        public string ApprovedBy { get; set; }
        public string ActionedBy { get; set; }
        public Originator Originator { get; set; }
        public DeliveryAction DeliveryAction { get; set; } 

    }
}
