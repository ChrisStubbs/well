namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum RouteStatus
    {
        Unknown = 0,

        [Description("Planned")] Planned = 1,

        [Description("In Progress")] InProgress = 2,

        [Description("Complete")] Complete = 3,

        [Description("Bypassed")] Bypassed = 4

    }
}