﻿namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum JobDetailStatus
    {
        [Description("Resolved")]
        Res = 1,

        [Description("Unresolved")]
        UnRes = 2,

        [Description("On Hold")]
        OnHld = 3,
    }
}
