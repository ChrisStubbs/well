namespace PH.Well.Domain.Enums
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Extensions;

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
        RouteInProgress = 5,

    }

    public static class WellStatusDescriptions
    {
        private static readonly Dictionary<int, string> descriptions;

        static WellStatusDescriptions()
        {
            descriptions = new Dictionary<int, string>();

            foreach (WellStatus item in System.Enum.GetValues(typeof(WellStatus)))
            {
                descriptions.Add((int)item, EnumExtensions.GetDescription(item));
            }

            descriptions[(int)WellStatus.Unknown] = null;
        }

        public static string Description(this WellStatus value)
        {
            return descriptions[(int)value];
        }
    }
}
