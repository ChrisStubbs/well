namespace PH.Well.Domain.Extensions
{
    using System.Collections.Generic;
    using Enums;
    using System.Linq;

    public static class ResolutionStatusExtensions
    {
        private static string isNotEditableMessage = null;
        private static readonly List<ResolutionStatus> EditableStatuses = new List<ResolutionStatus>
        {
            ResolutionStatus.DriverCompleted,
            ResolutionStatus.ActionRequired,
            ResolutionStatus.PendingSubmission,
            ResolutionStatus.PendingApproval,
            ResolutionStatus.ManuallyCompleted,
            ResolutionStatus.ApprovalRejected,
        };

        public static bool IsEditable(this ResolutionStatus resolutionStatus)
        {
            return EditableStatuses.Contains(resolutionStatus);
        }

        public static string IsNotEditableMessage()
        {
            if (isNotEditableMessage == null)
            {
                var all = EditableStatuses.Select(p => p.Description).ToList();

                isNotEditableMessage = "It is only possible to edit jobs if it has one of the following Resolution Status': " +
                    $"{string.Join(", ", all.Take(all.Count - 1))} or {all.Last()}";
            }

            return isNotEditableMessage;
        }
    }
}