using System;
using System.Collections.Generic;
using PH.Well.Domain;

namespace PH.Well.Services
{
    public interface IDateThresholdService
    {
        DateTime RouteGracePeriodEnd(DateTime routeDate, int branchId);
        DateTime GracePeriodEnd(DateTime routeDate, int branchId, int royaltyCode);
        IList<DateThreshold> GetAll();

        void Update(DateThreshold dateThreshold);
    }
}