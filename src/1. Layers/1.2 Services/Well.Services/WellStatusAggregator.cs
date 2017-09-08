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
            //TODO validate requirement for this 
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

        public WellStatus Aggregate(params WellStatus[] wellStatuses)
        {
            List<WellStatus> uniqueStatus = wellStatuses.Distinct().ToList();

            var anyCompleted = uniqueStatus.Any(x => x == WellStatus.Complete || x == WellStatus.CompleteWithBypass ||
                                                     x == WellStatus.Bypassed);

            var anyInProgress = uniqueStatus.Any(x => x == WellStatus.RouteInProgress || x == WellStatus.Invoiced);

            var anyNotStarted = uniqueStatus.Any(x => x == WellStatus.Planned || x == WellStatus.Unknown);

            if (anyCompleted)
            {
                if (anyInProgress || anyNotStarted)
                {
                    return WellStatus.RouteInProgress;
                }
                return WellStatus.Complete;
            }

            if (anyInProgress)
            {
                return WellStatus.RouteInProgress;
            }

            return WellStatus.Planned;
        }
    }
}
