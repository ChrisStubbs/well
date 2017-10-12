using System;

namespace PH.Well.Domain.ValueObjects
{
    public class AppSearchRouteItem : AppSearchItem
    {
        public int Id { get;  }
        public string RouteNumber { get; }

        public DateTime Date { get;  }

        public string DriverName { get; }

        public AppSearchRouteItem(int id,string routeNumber,string driverName,DateTime date):base(AppSearchItemType.Route)
        {
            Id = id;
            RouteNumber = routeNumber;
            DriverName = driverName;
            Date = date;
        }
    }
}