using System;

namespace PH.Well.Services
{
    public interface IDateThresholdService
    {
        DateTime BranchGracePeriodEndDate(DateTime routeDate, int branchId);
        DateTime GracePeriodEndDate(DateTime routeDate, int branchId, int royaltyCode);
    }
}