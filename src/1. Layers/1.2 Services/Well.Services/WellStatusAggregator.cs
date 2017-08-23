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
        public WellStatus Aggregate(IEnumerable<WellStatus> wellStatuses, AggregationType aggregationType)
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

        public WellStatus Aggregate(IEnumerable<LineItem> lineItems, AggregationType aggregationType)
        {
            WellStatus result = WellStatus.Unknown;
            // Get all the statuses of all the exceptions for these items
            var statuses = lineItems.Select(x => Aggregate(x.LineItemActions, aggregationType)).Distinct().ToList();
            if (statuses.Contains(ResolutionStatus.Closed))
            {
                
            }
            return result;
        }


        public ResolutionStatus Aggregate(IEnumerable<LineItemAction> lineItemActions, AggregationType aggregationType)
        {
            // TODO: DIJ - Chris we need to wire this up (I may be able to look at it later Wednesday)
            // Get all the status from the specified lineItemActions
            //lineItemActions.Select(x=>x.)
            return ResolutionStatus.Invalid;
        }
    }
}
