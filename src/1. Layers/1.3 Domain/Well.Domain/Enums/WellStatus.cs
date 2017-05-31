namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum WellStatus
    {
        Unknown = 0,

        [Description("Planned")]
        Planned = 1,

        [Description("Invoiced")]
        Invoiced = 2,

        [Description("Complete")]
        Complete = 3,

        [Description("Bypassed")]
        Bypassed = 4,

        [Description("In Progress")]
        InProgress = 5,

    }
}
