namespace PH.Well.Domain.Extensions
{
    using System;
    using System.Linq;
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

}