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

        [Description("Route Planned")]
        RoutePlanned = 5,

        [Description("Route In Progress")]
        RouteInProgress = 6,

        [Description("Route Complete")]
        RouteComplete = 7,

        [Description("Route Bypassed")]
        RouteBypassed = 4

    }
}
