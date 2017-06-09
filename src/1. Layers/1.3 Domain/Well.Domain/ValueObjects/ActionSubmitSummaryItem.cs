namespace PH.Well.Domain.ValueObjects
{
    using System;

    public class ActionSubmitSummaryItem
    {
        public string Identifier { get; set; }
        public int NoOfItems { get; set; }
        public int Qty { get; set; }
        public Decimal Value { get; set; }
    }
}