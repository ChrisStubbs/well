namespace PH.Well.Api.Models
{
    using System.Collections.Generic;

    public class DeliveryLineUpdateModel
    {
        public DeliveryLineUpdateModel()
        {
            Damages = new List<DamageUpdateModel>();
        } 

        public int JobId { get; set; }
        public int LineNumber { get; set; }
        public List<DamageUpdateModel> Damages { get; set; }
        public int ShortQuantity { get; set; }
    }
}