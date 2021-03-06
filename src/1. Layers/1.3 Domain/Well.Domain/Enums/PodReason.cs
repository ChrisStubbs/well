﻿namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum PodReason
    {
        [Description("Not Defined")]
        NotDefined = 0,
        [Description("Damaged")]
        Damaged = 1,
        [Description("Refused")]
        Refused = 2,
        [Description("Delivery failure")]
        DeliveryFailure = 3,
        [Description("Unable to offload")]
        UnableToOffload = 4,
        [Description("Picking error")]
        PickingError = 5
    }
}