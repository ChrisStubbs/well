using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Domain;

namespace PH.Well.Services.Contracts
{
    public interface IRouteService
    {
        /// <summary>
        /// Determine status and update route 
        /// </summary>
        /// <param name="routeId">PK of route to check</param>
        /// <returns>true if the current status changed</returns>
        bool ComputeWellStatus(int routeId);

        /// <summary>
        /// Determine status and update route 
        /// </summary>
        /// <param name="route">route</param>
        /// <returns>true if the current status changed</returns>
        bool ComputeWellStatus(RouteHeader route);
    }
}
