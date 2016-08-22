namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using System.Linq;

    public class DeliveryLine
    {
        public DeliveryLine()
        {
            Damages = new List<Damage>();
        }

        public int Id { get; set; }
        public int LineNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public int Value { get; set; }
        public int InvoicedQuantity { get; set; }
        public int ShortQuantity { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public List<Damage> Damages { get; set; }

        public int DamagedQuantity => Damages.Sum(d => d.Quantity);
        public int DeliveredQuantity => InvoicedQuantity - ShortQuantity - DamagedQuantity;
    }
}
