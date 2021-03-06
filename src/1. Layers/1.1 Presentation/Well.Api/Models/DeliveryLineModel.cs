﻿namespace PH.Well.Api.Models
{
    using System.Collections.Generic;

    public class DeliveryLineModel
    {
        public DeliveryLineModel()
        {
            this.Damages = new List<DamageModel>();
            this.Actions = new List<ActionModel>();
        }

        public int JobId { get; set; }

        public int JobDetailId { get; set; }

        public int LineNo { get; set; }

        public string ProductCode { get; set; }

        public string ProductDescription { get; set; }

        public string Value { get; set; }

        public int InvoicedQuantity { get; set; }

        public int DeliveredQuantity { get; set; }

        public int ShortQuantity { get; set; }

        public int DamagedQuantity { get; set; }

        public string LineDeliveryStatus { get; set; }

        public int JobDetailReasonId { get; set; }

        public int JobDetailSourceId { get; set; }

        public int ShortsActionId { get; set; }

        public string JobDetailReason { get; set; }

        public string JobDetailSource { get; set; }

        public string ShortsAction { get; set; }

        public bool IsHighValue { get; set; }

        public List<DamageModel> Damages { get; set; }

        public List<ActionModel> Actions { get; set; }
    }
}
