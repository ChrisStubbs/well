namespace PH.Well.Api.Mapper
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Extensions;
    using Domain;
    using Domain.Enums;
    using Models;

    using PH.Well.Api.Mapper.Contracts;

    public class RouteModelsMapper : IMapper<IEnumerable<RouteHeader>, IEnumerable<RouteModel>>, IRouteModelsMapper
    {
        public IEnumerable<RouteModel> Map(IEnumerable<RouteHeader> source)
        {
            var routeModels = new List<RouteModel>();

            foreach (var routeHeader in source)
            {
                var model = new RouteModel
                {
                    Route = routeHeader.RouteNumber,
                    DriverName = routeHeader.DriverName,
                    TotalDrops = routeHeader.Stops.Count,
                    DeliveryCleanCount = routeHeader.Stops.Count(x=> x.StopPerformanceStatusCodeId == (int)PerformanceStatus.Compl),
                    DeliveryExceptionCount = routeHeader.Stops.Count(x => x.StopPerformanceStatusCodeId != (int)PerformanceStatus.Compl),
                    RouteStatus =   StringExtensions.GetEnumDescription(routeHeader.RouteStatus),
                    DateTimeUpdated = routeHeader.DateUpdated.ToDashboardDateFormat()
                };
                routeModels.Add(model);
            }

            return routeModels;
        }
    }
}