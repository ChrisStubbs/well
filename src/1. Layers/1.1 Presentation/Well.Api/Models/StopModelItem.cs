﻿namespace PH.Well.Api.Models
{

    public class StopModelItem
    {
        public int JobId { get; set; }
        public string Invoice { get; set; }
        public int InvoiceId { get; set; }
        public string @Type { get; set; }
        public string JobTypeAbbreviation { get; set; }
        public int JobTypeId { get; set; }
        public string Account { get; set; }
        public int AccountID { get; set; }
        public int JobDetailId { get; set; }
        public string Product { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public int Invoiced { get; set; }
        public int Delivered { get; set; }
        public int Damages { get; set; }
        public int Shorts { get; set; }
        public int Bypassed { get; set; }
        public bool Checked { get; set; }
        public bool HighValue { get; set; }
        public string BarCode { get; set; }
        public int LineItemId { get; set; }
        public string Resolution { get; set; }
        public int ResolutionId { get; set; }
        public bool HasUnresolvedActions { get; set; }
        public int GrnProcessType { get; set; }
        public string GrnNumber { get; set; }
        public bool CanEdit => string.IsNullOrEmpty(this.CanEditReason);
        public string CanEditReason { get; set; }
        public int LocationId { get; set; }
        public bool CompletedOnPaper { get; set; }
        public bool HasLineItemActions { get; set; }

        public int UpliftAction { get; set; }
    }
}