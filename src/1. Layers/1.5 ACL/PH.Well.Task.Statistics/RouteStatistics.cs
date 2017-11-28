using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Well.Common.Contracts;
using PH.Well.Domain.Enums;
using PH.Well.Services.Contracts;

namespace PH.Well.Task.Statistics
{
    public class RouteStatistics : IRouteStatistics
    {
        private readonly IRouteService routeService;
        private readonly ILogger logger;

        public RouteStatistics(IRouteService routeService,ILogger logger)
        {
            this.routeService = routeService;
            this.logger = logger;
        }

        public void UpdateRouteStatistics()
        {
            var branches = Enum.GetValues(typeof(Branches)).Cast<Branches>();
            foreach (var branch in branches)
            {
                try
                {
                    var branchId = (int)branch;
                    routeService.UpdateRouteStatistics(branchId);
                    logger.LogDebug($"Updated route statistics for branch {branch}");
                }
                catch (Exception e)
                {
                    logger.LogError("Error updating route statistics", e);
                }

            }
        }
    }
}
