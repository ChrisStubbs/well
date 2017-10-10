namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Enums;

    public class EditLineItemException
    {
        public EditLineItemException()
        {
            Exceptions = new List<EditLineItemExceptionDetail>();
            LineItemActions = new List<LineItemAction>();
        }
        public int Id { get; set; }
        public int JobId { get; set; }
        public int ResolutionId { get; set; }
        public string ResolutionStatus { get; set; }
        public string AccountCode { get; set; }
        public string Invoice { get; set; }
        public string JobTypeDescription { get; set; }
        public JobType JobType { get; set; }
        public string ProductNumber { get; set; }
        public string DriverReason { get; set; }
        public string Product { get; set; }
        public decimal Value { get; set; }
        public int? Invoiced { get; set; }
        public int? Delivered => Invoiced - (Damages + Shorts + Bypass);
        public int Damages { get; set; }
        public int Shorts { get; set; }
        public int Bypass { get; set; }
        public int Quantity { get; set; }
        public bool CanEditActions => string.IsNullOrEmpty(this.CanEditActionsReason);
        public string CanEditActionsReason { get; set; }
        public string Resolution { get; set; }
        public bool IsProofOfDelivery { get; set; }
        public bool HasUnresolvedActions => LineItemActions.Any(x => x.DeliveryAction == DeliveryAction.NotDefined);
        public IList<EditLineItemExceptionDetail> Exceptions { get; set; }
        public IList<LineItemAction> LineItemActions { get; set; }
    }

    public class EditLineItemExceptionDetail
    {
        public int Id { get; set; }
        public int LineItemId { get; set; }
        public int Quantity { get; set; }
        public string Originator { get; set; }
        public string Exception { get; set; }
        public string Action { get; set; }
        public string Source { get; set; }
        public string Reason { get; set; }
        public DateTime? Erdd { get; set; }
        public string ActionedBy { get; set; }
        public string ApprovedBy { get; set; }
        public IList<string> Comments { get; set; }

    }

}
