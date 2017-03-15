namespace PH.Well.Api.Models
{
    using System.Collections.Generic;

    public class DeliveryActionModel
    {
        public DeliveryActionModel()
        {
            Lines = new List<DeliveryLineModel>();
        }
        public int JobId { get; set; }
        public decimal TotalCreditThreshold { get; set; }
        public List<DeliveryLineModel> Lines { get; set; }
    }
}
