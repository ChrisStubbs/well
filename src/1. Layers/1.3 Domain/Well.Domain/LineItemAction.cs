namespace PH.Well.Domain
{
    using System;
    using System.Collections.Generic;
    using Enums;

    public class LineItemAction: Entity<int>
    {
        public LineItemAction()
        {
            Comments = new List<LineItemActionComment>();
        }
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
        public IEnumerable<LineItemActionComment> Comments { get; set; }
        

        public bool HasChanges(LineItemAction item)
        {
            return ExceptionType != item.ExceptionType
                   || Quantity != item.Quantity
                   || Source != item.Source
                   || Reason != item.Reason
                   || ReplanDate != item.ReplanDate
                   || SubmittedDate != item.SubmittedDate
                   || ApprovalDate != item.ApprovalDate
                   || ApprovedBy != item.ApprovedBy
                   || ActionedBy != item.ActionedBy
                   || Originator != item.Originator
                   || DeliveryAction != item.DeliveryAction;
        }


    }
}
