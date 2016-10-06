namespace PH.Well.Api.Models
{
    using System.Collections.Generic;

    public class DeliveryLineActionsModel
    {
        public DeliveryLineActionsModel()
        {
            DraftActions = new List<ActionModel>();
        }

        public int JobDetailId { get; set; }
        public List<ActionModel> DraftActions { get; set; }
    }
}