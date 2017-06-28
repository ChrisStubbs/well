namespace PH.Well.Api.Models
{
    using System.Collections.Generic;
    using Domain.Contracts;
    using Domain.Enums;

    public class BulkEditPatchRequest : ILineItemActionResolution
    {
        public DeliveryAction DeliveryAction { get; set; }
        public JobDetailSource Source { get; set; }
        public JobDetailReason Reason { get; set; }
        public IEnumerable<int> JobIds { get; set; }
        public IEnumerable<int> LineItemIds { get; set; }

    }
}