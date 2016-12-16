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
                var model = new RouteModel{
                    Route = routeHeader.RouteNumber,
                    RouteDate = routeHeader.RouteDate.Value,
                    DriverName = routeHeader.DriverName,
                    TotalDrops = routeHeader.Stops.Count,
                    DeliveryCleanCount = routeHeader.CleanJobs,
                    DeliveryExceptionCount = routeHeader.ExceptionJobs,
                    RouteStatus =   StringExtensions.GetEnumDescription(routeHeader.RouteStatus),
                    DateTimeUpdated = routeHeader.DateUpdated,
                    RouteOwnerId = routeHeader.RouteOwnerId
                };
                routeModels.Add(model);
            }

            return routeModels;
        }
    }
}