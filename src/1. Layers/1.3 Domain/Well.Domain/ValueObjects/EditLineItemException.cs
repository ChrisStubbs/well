namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;

    public class EditLineItemException
    {
        public int Id { get; set; }
        public string ProductNumber { get; set; }
        public string Product { get; set; }
        public string Originator { get; set; }
        public string Exception { get; set; }
        public int? Invoiced { get; set; }
        public int? Delivered { get; set; }
        public int Quantity { get; set; }
        public string Action { get; set; }
        public string Source { get; set; }
        public string Reason { get; set; }
        public DateTime? Erdd { get; set; }
        public string ActionedBy { get; set; }
        public string ApprovedBy { get; set; }
        public IList<string> Comments { get; set; }
    }
}
