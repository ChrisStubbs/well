namespace PH.Well.Domain.Enums
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Extensions;

    public enum JobType
    {
        Unknown = 0,

        [Description("DEL-TOB")]
        Tobacco = 1,

        [Description("DEL-AMB")]
        Ambient = 2,

        [Description("DEL-ALC")]
        Alcohol = 3,

        [Description("DEL-CHL")]
        Chilled = 4,

        [Description("DEL-FRZ")]
        Frozen = 5,

        [Description("DEL-DOC")]
        Documents = 6,

        [Description("UPL-SAN")]
        SandwichUplift = 7,

        [Description("UPL-GLO")]
        GlobalUplift = 8,

        [Description("UPL-ASS")]
        AssetsUplift = 9,

        [Description("UPL-STD")]
        StandardUplift = 10,

        [Description("NotDef")]
        NotDefined = 11
    }

    public static class JobTypeDescriptions
    {
        private static readonly Dictionary<int, string> descriptions;
        private static readonly Dictionary<string, JobType> values;

        static JobTypeDescriptions()
        {
            descriptions = new Dictionary<int, string>();
            values = new Dictionary<string, JobType>(StringComparer.InvariantCultureIgnoreCase);

            foreach (JobType item in System.Enum.GetValues(typeof(JobType)))
            {
                var desc = EnumExtensions.GetDescription(item);
                descriptions.Add((int)item, desc);

                values.Add(desc, item);
            }
        }

        public static string Description(this JobType value)
        {
            return descriptions[(int)value];
        }

        public static JobType FromDescription(string value)
        {
            JobType result = JobType.Unknown;

            values.TryGetValue(value, out result);

            return result;
        }
    }
}
