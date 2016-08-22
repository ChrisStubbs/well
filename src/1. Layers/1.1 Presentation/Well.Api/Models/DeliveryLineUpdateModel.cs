namespace PH.Well.Api.Models
{
    using System.Collections.Generic;

    public class DeliveryLineUpdateModel
    {
        public DeliveryLineUpdateModel()
        {
            Damages = new List<DamageModel>();
        } 

        public int JobId { get; set; }
        public int LineNumber { get; set; }
        public List<DamageModel> Damages { get; set; }
        public int ShortQuantity { get; set; }
    }
}