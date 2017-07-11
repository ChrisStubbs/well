namespace PH.Well.Domain.Extensions
{
    using System.Collections.Generic;
    using Enums;

    public static class ResolutionStatusExtensions
    {
        private static readonly List<ResolutionStatus> EditableStatuses = new List<ResolutionStatus>
        {
            ResolutionStatus.DriverCompleted,
            ResolutionStatus.ActionRequired,
            ResolutionStatus.PendingSubmission,
            ResolutionStatus.PendingApproval,
            ResolutionStatus.CompletedByWell,
        };

        public static bool IsEditable(this ResolutionStatus resolutionStatus)
        {
            return EditableStatuses.Contains(resolutionStatus);
        }
    }
}