using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain;
using PH.Well.Domain.Enums;

namespace PH.Well.Services.Contracts
{
    /// <summary>
    /// What level do we want to aggregate statuses to?
    /// - This is required due to custom sub-sets of WellStatuses used at various levels
    /// </summary>
    public enum AggregationType
    {
        Job,
        Stop,
        Route,
        Invoice,
        Location   
    }

    /// <summary>
    /// Aggregates multiple WellStatus values to a single result
    /// </summary>
    public interface IWellStatusAggregator
    {
        /// <summary>
        /// Aggregate a set of WellStatus into a single summary status
        /// </summary>
        /// <param name="wellStatuses"></param>
        /// <param name="aggregationType"></param>
        /// <returns></returns>
        WellStatus Aggregate(IEnumerable<WellStatus> wellStatuses, AggregationType aggregationType);

        /// <summary>
        /// Aggregate a set of LineItems (and their LineItemActions) into a single summary status
        /// </summary>
        /// <param name="lineItems"></param>
        /// <param name="aggregationType"></param>
        /// <returns></returns>
        WellStatus Aggregate(IEnumerable<LineItem> lineItems, AggregationType aggregationType);

        /// <summary>
        /// Aggregate a set of line item actions into a single summary status
        /// </summary>
        /// <param name="lineItemActions"></param>
        /// <param name="aggregationType"></param>
        /// <returns></returns>
        ResolutionStatus Aggregate(IEnumerable<LineItemAction> lineItemActions, AggregationType aggregationType);
    }
}
