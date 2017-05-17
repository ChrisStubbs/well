namespace PH.Well.Domain.ValueObjects
{
    using System;

    public class LineItemActionUpdate
    {
        public string ExceptionType { get; set; }

        public int Quantity { get; set; }

        public int LineItemId { get; set; }

    }
}
