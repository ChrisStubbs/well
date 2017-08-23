using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Services.Contracts
{
    public interface ILocationService
    {
        /// <summary>
        /// Force the location to recalculate its status from the child invoices
        /// </summary>
        /// <param name="locationId">PK of location to check</param>
        /// <returns>true if the current status changed</returns>
        bool ComputeWellStatus(int locationId);
    }
}
