using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PH.Well.Services.Contracts
{
    public interface IRouteService
    {
        /// <summary>
        /// Force the stop to recalculate its status from the child stops
        /// </summary>
        /// <param name="routeId">PK of route to check</param>
        /// <returns>true if the current status changed</returns>
        bool ComputeWellStatus(int routeId);
    }
}
