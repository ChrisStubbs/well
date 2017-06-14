using System;

namespace PH.Well.Services
{
    public interface IDateThresholdService
    {
        DateTime EarliestCreditDate(DateTime routeDate, int branchId);
    }
}