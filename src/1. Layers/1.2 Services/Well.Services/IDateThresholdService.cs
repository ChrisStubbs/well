using System;

namespace PH.Well.Services
{
    public interface IDateThresholdService
    {
        DateTime EarliestSubmitDate(DateTime routeDate, int branchId);
    }
}