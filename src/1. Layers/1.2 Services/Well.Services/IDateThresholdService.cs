using System;

namespace PH.Well.Services
{
    public interface IDateThresholdService
    {
        DateTime RouteGracePeriodEnd(DateTime routeDate, int branchId);
        DateTime GracePeriodEnd(DateTime routeDate, int branchId, int royaltyCode);
    }
}