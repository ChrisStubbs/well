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
        /// <returns>the route header with any amendments</returns>
        RouteHeader ComputeWellStatus(int routeId);

        /// <summary>
        /// Determine status and update route 
        /// </summary>
        /// <param name="route">route</param>
        /// <returns>the route header with any amendments</returns>
        RouteHeader ComputeWellStatus(RouteHeader route);


        /// <summary>
        /// Determine status update route and send notification if the status changes from Completed
        /// </summary>
        /// <returns></returns>
        RouteHeader ComputeWellStatusAndNotifyIfChangedFromCompleted(int routeId);
    }
}
