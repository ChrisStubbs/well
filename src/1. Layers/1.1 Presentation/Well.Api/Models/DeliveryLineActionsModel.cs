namespace PH.Well.Api.Models
{
    using System.Collections.Generic;
    using Domain.Enums;

    public class DeliveryLineActionsModel
    {
        public int JobDetailId { get; set; }
        public List<ActionModel> DraftActions { get; set; }
    }
}