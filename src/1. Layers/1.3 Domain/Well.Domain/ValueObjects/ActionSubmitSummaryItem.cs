namespace PH.Well.Domain.ValueObjects
{
    using System;

    public class ActionSubmitSummaryItem
    {
        public string Identifier { get; set; }
        public decimal TotalCreditValue { get; set; }
        public decimal TotalActionValue { get; set; }
        public int TotalCreditQty { get; set; }
        public int TotalQty { get; set; }
      
    }
}