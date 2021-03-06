﻿namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum PodDeliveryAction
    {
        [Description("Not Defined")]
        NotDefined = 0,

        //[Description("Credit")]
        //Credit = 1,

        [Description("Credit And Reorder")]
        CreditAndReorder = 2,

        [Description("Replan In Roadnet")]
        ReplanInRoadnet = 3,

        [Description("Replan In TranSend")]
        ReplanInTranSend = 4,

        [Description("Replan In The Queue")]
        ReplanInTheQueue = 5,

        [Description("Reject")]
        Reject = 6,

        [Description("Pod")]
        Pod = 7
    }
}