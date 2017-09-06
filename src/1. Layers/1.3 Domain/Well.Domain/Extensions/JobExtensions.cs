namespace PH.Well.Domain.Extensions
{
    using System.Linq;
    using Enums;
    using ValueObjects;

    public static class JobExtensions
    {
        public static string Identifier(this Job job)
        {
            return $"{job.PhAccount} - {job.PickListRef} - {job.JobTypeCode}";
        }

        public static bool IsDocumentDelivery(this Job job)
        {
            return job.PickListRef == Job.DocumentPickListReference;
        }

        public static bool CanWeUpdateJobOnImport(this Job job)
        {
            switch (job.WellStatus)
            {
                case WellStatus.Complete:
                    return false;
                case WellStatus.Bypassed:
                    return true;
                default:
                    return true;
            }

        }

        public static bool HasUnresolvedActions(this Job job, int? lineItemId = null)
        {
            var data = job.LineItems.AsQueryable();

            if (lineItemId.HasValue)
            {
                data = data.Where(x => x.Id == lineItemId);
            }

            return CalculateHasUnresolvedActions(
                job.ResolutionStatus,
                data
                    .SelectMany(p => p.LineItemActions)
                    .Any(p => p.DeliveryAction == DeliveryAction.NotDefined));
        }

        private static bool CalculateHasUnresolvedActions(ResolutionStatus rs, bool hasNotDefinedActionsCount)
        {
            if ((rs & ResolutionStatus.Closed) == ResolutionStatus.Closed)
            {
                return false;
            }

            return hasNotDefinedActionsCount;
        }

        public static bool HasUnresolvedActions(this ActivitySourceDetail value)
        {
            return CalculateHasUnresolvedActions(value.ResolutionStatus, value.HasNoDefinedActions);
        }

        public static WellStatus ToWellStatus(this JobStatus jobStatus)
        {
            switch (jobStatus)
            {
                case JobStatus.NotDefined:
                    return WellStatus.Unknown;
                case JobStatus.AwaitingInvoice:
                    return WellStatus.Planned;
                case JobStatus.InComplete:
                    return WellStatus.Invoiced;
                case JobStatus.Bypassed:
                    return WellStatus.Bypassed;
                default:
                   return WellStatus.Complete;
            }
        }
    }
}