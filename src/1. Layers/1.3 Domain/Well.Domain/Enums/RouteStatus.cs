namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;

    public enum RouteStatus
    {
        Unknown = 0,

        [Description("Route Planned")] Planned = 1,

        [Description("Route In Progress")] InProgress = 2,

        [Description("Route Complete")] Complete = 3,

        [Description("Route Bypassed")] Bypassed = 4

    }
}