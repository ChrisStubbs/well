using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PH.Well.Domain;

namespace PH.Well.Services
{
    public interface IDateThresholdService
    {
        DateTime RouteGracePeriodEnd(DateTime routeDate, int branchId);
        Task<DateTime> RouteGracePeriodEndAsync(DateTime routeDate, int branchId);
        DateTime GracePeriodEnd(DateTime routeDate, int branchId, int royaltyCode);
        Task<DateTime> GracePeriodEndAsync(DateTime routeDate, int branchId, int royaltyCode);
        IList<DateThreshold> GetAll();
        void UpdateDateThresholdAllDatabases(DateThreshold dateThreshold);
    }
}