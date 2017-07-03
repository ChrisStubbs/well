namespace PH.Well.Domain.Enums
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Extensions;

    //todo get rid
    public enum RouteStatus
    {
        Unknown = 0,

        [Description("Route Planned")] Planned = 5,

        [Description("Route In Progress")] InProgress = 6,

        [Description("Route Complete")] Complete = 7,

        [Description("Route Bypassed")] Bypassed = 8
    }

    public static class RouteStatusDescriptions
    {
        private static readonly Dictionary<int, string> descriptions;

        static RouteStatusDescriptions()
        {
            descriptions = new Dictionary<int, string>();

            foreach (RouteStatus item in System.Enum.GetValues(typeof(RouteStatus)))
            {
                descriptions.Add((int)item, EnumExtensions.GetDescription(item));
            }

            descriptions[(int)RouteStatus.Unknown] = null;
        }

        public static string Description(this RouteStatus value)
        {
            return descriptions[(int)value];
        }
    }
}