using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Services.Contracts
{
    public interface IActivityService
    {
        /// <summary>
        /// Force the activity/invoice to recalculate its status from the child invoices
        /// </summary>
        /// <param name="activityId">PK of activity/invoice to check</param>
        /// <returns>true if the current status changed</returns>
        bool ComputeWellStatus(int activityId);
    }
}
