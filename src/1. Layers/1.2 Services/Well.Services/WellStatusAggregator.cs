using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain;
using PH.Well.Domain.Enums;
using PH.Well.Services.Contracts;

namespace PH.Well.Services
{
    public class WellStatusAggregator : IWellStatusAggregator
    {
        public WellStatus Aggregate(params ResolutionStatus.eResolutionStatus[] resolutionStatuses)
        {
            if (resolutionStatuses.Any(x => x == ResolutionStatus.eResolutionStatus.Invalid))
            {
                return WellStatus.RouteInProgress;
            }

            if (resolutionStatuses.All(x => x == ResolutionStatus.eResolutionStatus.Imported))
            {
                return WellStatus.Planned;
            }

            return WellStatus.Complete;
        }

        public WellStatus Aggregate(AggregationType aggregationType, params WellStatus[] wellStatuses)
        {
            WellStatus result = WellStatus.Unknown;
            List<WellStatus> uniqueStatus = wellStatuses.Distinct().ToList();
            if (uniqueStatus.Contains(WellStatus.Planned))
            {
                result = WellStatus.Planned;
            }
            if (uniqueStatus.Contains(WellStatus.Invoiced))
            {
                result = aggregationType == AggregationType.Route ? WellStatus.Planned : WellStatus.Invoiced;
            }
            if (uniqueStatus.Contains(WellStatus.RouteInProgress))
            {
                result = WellStatus.RouteInProgress;
            }
            if (uniqueStatus.Contains(WellStatus.Complete))
            {
                result = WellStatus.Complete;
            }
            if (uniqueStatus.Contains(WellStatus.Bypassed))
            {
                if (result == WellStatus.Complete)
                {
                    result = WellStatus.CompleteWithBypass;
                }
                else
                {
                    result = WellStatus.Bypassed;
                }
            }
            return result;
        }

        public WellStatus Aggregate(Job job, params ResolutionStatus.eResolutionStatus[] resolutionStatuses)
        {
            throw new NotImplementedException();
        }
    }
}
