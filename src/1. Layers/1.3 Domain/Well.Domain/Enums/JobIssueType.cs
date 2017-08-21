using System.Collections.Generic;
using System.ComponentModel;
using PH.Well.Domain.Extensions;

namespace PH.Well.Domain.Enums
{
    public enum JobIssueType
    {
        [Description("All")]
        All = 0,

        [Description("Action Required")]
        ActionRequired = 1,

        [Description("Pending Submission")]
        PendingSubmission = 2,

        [Description("Missing GRN")]
        MissingGRN = 4
    }

    public static class JobIssueTypeDescriptions
    {
        private static readonly Dictionary<int, string> descriptions;

        static JobIssueTypeDescriptions()
        {
            descriptions = new Dictionary<int, string>();

            foreach (JobIssueType item in System.Enum.GetValues(typeof(JobIssueType)))
            {
                descriptions.Add((int)item, EnumExtensions.GetDescription(item));
            }
        }

        public static string Description(this JobIssueType value)
        {
            return descriptions[(int)value];
        }
    }
}
