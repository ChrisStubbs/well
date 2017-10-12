using System;

namespace PH.Well.Domain.ValueObjects
{
    public interface IAppSearchItem
    {
        AppSearchItemType ItemType { get; }
    }

    public abstract class AppSearchItem : IAppSearchItem
    {
        public AppSearchItemType ItemType { get; }

        protected AppSearchItem(AppSearchItemType appSearchItem)
        {
            ItemType = appSearchItem;
        }
    }

    public class AppSearchInvoiceItem : AppSearchItem
    {
        /// <summary>
        /// Activity Id
        /// </summary>
        public int Id { get;}

        public string Type { get;}    

        public string DocumentNumber { get; }

        public string LocationName { get; }

        public DateTime Date { get; }

        public AppSearchInvoiceItem(int id, string documentNumber, string type, string locationName,
            DateTime date) : base(AppSearchItemType.Invoice)
        {
            Id = id;
            DocumentNumber = documentNumber;
            Type = type;
            LocationName = locationName;
            Date = date;
        }
    }

    public class AppSearchLocationItem : AppSearchItem
    {
        /// <summary>
        /// Location id
        /// </summary>
        public int Id { get;}

        public string Name { get; }

        public string AccountNumber { get; }

        public AppSearchLocationItem(int id,string name,string accountName):base(AppSearchItemType.Location)
        {
            Id = id;
            Name = name;
            AccountNumber = accountName;
        }
    }

    public class AppSearchRouteItem : AppSearchItem
    {
        public string RouteNumber { get; }

        public DateTime Date { get;  }

        public string DriverName { get; }

        public AppSearchRouteItem(string routeNumber,string driverName,DateTime date):base(AppSearchItemType.Route)
        {
            RouteNumber = routeNumber;
            DriverName = driverName;
            Date = date;
        }
    }


    public enum AppSearchItemType
    {
        Invoice = 1,
        Location = 2,
        Route = 3
    }
}
