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
    /// Aggregates multiple WellStatus values to a single result
    /// </summary>
    public interface IWellStatusAggregator
    {
        /// <summary>
        /// Aggregate a set of WellStatus into a single summary status
        /// </summary>
        /// <param name="wellStatuses"></param>
        /// <returns></returns>
        WellStatus Aggregate(params WellStatus[] wellStatuses);

        /// <summary>
        /// Aggregate a set of WellStatus into a single summary status from resolution statuses
        /// </summary>
        /// <param name="resolutionStatuses"></param>
        /// <returns></returns>
        WellStatus Aggregate(params ResolutionStatus.eResolutionStatus[] resolutionStatuses);
    }
}
