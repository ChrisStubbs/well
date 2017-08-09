using System.Collections.Generic;

namespace PH.Well.Domain.Enums
{
    using System.ComponentModel;
    using PH.Well.Domain.Extensions;

    public enum ExceptionType
    {
        [Description("Not Defined")]
        NotDefined = 0,

        [Description("Short")]
        Short = 1,

        [Description("Bypass")]
        Bypass = 2,

        [Description("Damage")]
        Damage = 3,

        [Description("Successful uplift")]
        Uplifted = 4
    }

    public static class ExceptionTypeDescriptions
    {
        private static readonly Dictionary<int, string> descriptions;

        static ExceptionTypeDescriptions()
        {
            descriptions = new Dictionary<int, string>();

            foreach (ExceptionType item in System.Enum.GetValues(typeof(ExceptionType)))
            {
                descriptions.Add((int)item, EnumExtensions.GetDescription(item));
            }
        }

        public static string Description(this ExceptionType value)
        {
            // Equivalent of previous implementation, will cause NotDefined Text display for actions that will have null description
            //string description = null;
            //descriptions.TryGetValue((int)value, out description);
            //return description;
            return descriptions[(int)value];
        }
    }
}
