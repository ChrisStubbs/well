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
        public WellStatus Aggregate(params WellStatus[] wellStatuses)
        {
            List<WellStatus> uniqueStatus = wellStatuses.Distinct().ToList();

            var anyCompleted = uniqueStatus.Any(x => x == WellStatus.Complete || x == WellStatus.Bypassed);

            var anyInProgress = uniqueStatus.Any(x => x == WellStatus.RouteInProgress);

            var anyNotStarted = uniqueStatus.Any(x => x == WellStatus.Planned || x == WellStatus.Unknown);

            var anyInvoiced = uniqueStatus.Any(x => x == WellStatus.Invoiced || x == WellStatus.Replanned);

            if (anyCompleted)
            {
                if (anyInProgress || anyNotStarted || anyInvoiced)
                {
                    return WellStatus.RouteInProgress;
                }

                if (wellStatuses.All(x => x == WellStatus.Bypassed))
                {
                    return WellStatus.Bypassed;
                }

                return WellStatus.Complete;
            }

            if (anyInProgress)
            {
                return WellStatus.RouteInProgress;
            }

            if (anyInvoiced)
            {
                return WellStatus.Invoiced;
            }

            return WellStatus.Planned;

        }
    }
}
