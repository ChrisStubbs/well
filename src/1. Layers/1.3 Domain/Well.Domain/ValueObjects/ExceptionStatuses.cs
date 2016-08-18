namespace PH.Well.Domain.ValueObjects
{
    using System.Collections.Generic;
    using Enums;

    public class ExceptionStatuses
    {
        public static List<PerformanceStatus> Statuses => new List<PerformanceStatus>()
        {
            PerformanceStatus.Incom,
            PerformanceStatus.Abypa,
            PerformanceStatus.Nbypa
        };
    }
}
