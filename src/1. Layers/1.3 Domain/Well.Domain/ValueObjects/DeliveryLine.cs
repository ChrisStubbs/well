namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using System.Linq;

    public class DeliveryLine
    {
        public DeliveryLine()
        {
            Damages = new List<Damage>();
            Actions = new List<JobDetailAction>();
        }

        public int JobDetailId { get; set; }

        public int JobId { get; set; }

        public int LineNo { get; set; }

        public string ProductCode { get; set; }

        public string ProductDescription { get; set; }

        public int Value { get; set; }

        public int InvoicedQuantity { get; set; }

        public int ShortQuantity { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public List<Damage> Damages { get; set; }

        public List<JobDetailAction> Actions { get; set; }

        public int JobDetailReasonId { get; set; }

        public int JobDetailSourceId { get; set; }

        public int DamagedQuantity => Damages.Sum(d => d.Quantity);

        public int DeliveredQuantity => InvoicedQuantity - ShortQuantity - DamagedQuantity;

        public bool IsClean => Damages.Sum(d => d.Quantity) + ShortQuantity == 0;
    }
}
