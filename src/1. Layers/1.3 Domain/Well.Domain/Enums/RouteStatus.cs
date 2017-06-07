namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;
    //todo get rid
    public enum RouteStatus
    {
        Unknown = 0,

        [Description("Route Planned")] Planned = 5,

        [Description("Route In Progress")] InProgress = 6,

        [Description("Route Complete")] Complete = 7,

        [Description("Route Bypassed")] Bypassed = 8

    }
}