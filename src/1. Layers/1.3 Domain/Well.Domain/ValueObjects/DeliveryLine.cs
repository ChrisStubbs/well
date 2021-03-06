﻿namespace PH.Well.Domain.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Enums;

    public class DeliveryLine
    {
        public DeliveryLine()
        {
            Damages = new List<Damage>();
        }

        public int JobDetailId { get; set; }

        public int JobId { get; set; }

        public int LineNo { get; set; }

        public string ProductCode { get; set; }

        public string ProductDescription { get; set; }

        public decimal Value { get; set; }

        public int InvoicedQuantity { get; set; }

        public int ShortQuantity { get; set; }

        public int DeliveredQuantity { get; set; }

        public int ShortsActionId { get; set; }

        public DeliveryAction ShortsAction => (DeliveryAction)ShortsActionId;

        public string LineDeliveryStatus { get; set; }

        public string Reason { get; set; }

        public string Status { get; set; }

        public List<Damage> Damages { get; set; }

        public int JobDetailReasonId { get; set; }

        public int JobDetailSourceId { get; set; }

        public int DamagedQuantity => Damages.Sum(d => d.Quantity);

        public bool IsClean => !IsException;

        private bool IsException => (DamagedQuantity + ShortQuantity > 0); 

        public bool CanSubmit => (ShortQuantity == 0 || ShortsAction != DeliveryAction.NotDefined) &&
                                 Damages.All(d => d.Quantity == 0 || d.DamageAction != DeliveryAction.NotDefined);

        public decimal CreditValueForThreshold()
        {
            var c = (this.ShortQuantity + DamagedQuantity) * this.Value;
            return c;
        }

        public bool IsHighValue { get; set; }

        public bool HasNoActions => ShortsAction == DeliveryAction.NotDefined &&
                                    Damages.All(d => d.DamageAction == DeliveryAction.NotDefined);

    }
}
