namespace PH.Well.Api.Models
{
    using System.Collections.Generic;

    public class DeliveryLineModel
    {
        public DeliveryLineModel()
        {
            Damages = new List<DamageModel>();
        }

        public int JobId { get; set; }
        public int LineNo { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string Value { get; set; }
        public int InvoicedQuantity { get; set; }
        public int DeliveredQuantity { get; set; }
        public int ShortQuantity { get; set; }
        public int DamagedQuantity { get; set; }

        public List<DamageModel> Damages { get; set; }

        public List<ActionModel> Actions { get; set; }
    }
}
