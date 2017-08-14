namespace PH.Well.Domain.Extensions
{
    using System;
    using Enums;

    public static class StopExtensions
    {

        public static string GetPreviously(this Stop currentStop, Stop originalStop)
        {
            if (originalStop.Identifier().Equals(currentStop.Identifier()))
            {
                return null;
            }

            return !originalStop.DeliveryDate.ToShortDateString().Equals(currentStop.DeliveryDate.ToShortDateString(), StringComparison.InvariantCultureIgnoreCase)
                    ? currentStop.Identifier()
                    : $"{currentStop.RouteHeaderCode} - {currentStop.DropId}";
        }

        public static string Identifier(this Stop stop)
        {
            return $"{stop.RouteHeaderCode} - {stop.DropId} - {stop.DeliveryDate.ToShortDateString()}";
        }

        public static bool HasStopBeenCompleted(this Stop stop)
        {
            return stop.WellStatus == WellStatus.Complete;
        }
    }

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
    }

}