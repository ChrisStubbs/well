namespace PH.Well.Api.Models
{
    public class RouteModel
    {
        public string Route { get; set; }

        public string DriverName { get; set; }

        public int TotalDrops { get; set; }

        public int DeliveryExceptionCount { get; set; }

        public int DeliveryCleanCount { get; set; }

        public string RouteStatus { get; set; }

        public string DateTimeUpdated { get; set; }
    }
}