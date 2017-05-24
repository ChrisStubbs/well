namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;

    public class EditLineItemException
    {
        public int Id { get; set; }
        public string ProductNumber { get; set; }
        public string Product { get; set; }
        public int? Invoiced { get; set; }
        public int? Delivered { get; set; }
        public int Quantity { get; set; }
        public IList<EditLineItemExceptionDetail> Exceptions { get; set; }
    }

    public class EditLineItemExceptionDetail
    {
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

    //public class EditLineItemException
    //{
    //    public int Id { get; set; }
    //    public int? LineItemActionId { get; set; }
    //    public string ProductNumber { get; set; }
    //    public string Product { get; set; }
    //    public string Originator { get; set; }
    //    public string Exception { get; set; }
    //    public int? Invoiced { get; set; }
    //    public int? Delivered { get; set; }
    //    public int Quantity { get; set; }
    //    public string Action { get; set; }
    //    public string Source { get; set; }
    //    public string Reason { get; set; }
    //    public DateTime? Erdd { get; set; }
    //    public string ActionedBy { get; set; }
    //    public string ApprovedBy { get; set; }
    //    public IList<string> Comments { get; set; }
    //    // public IList<EditLIneItemExceptionDetail> Exceptions { get; set; }
    //}

}
