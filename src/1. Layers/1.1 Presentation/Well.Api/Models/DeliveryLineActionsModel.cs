namespace PH.Well.Api.Models
{
    using System.Collections.Generic;
    using Domain.Enums;

    public class DeliveryLineActionsModel
    {
        public int DeliveryLineId { get; set; }
        public List<ActionModel> Actions { get; set; }
    }
}