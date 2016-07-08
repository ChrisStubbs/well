namespace PH.Well.Repositories
{
    public struct StoredProcedures
    {
        //widgets
        public static string WidgetStatsGet = "WidgetStats_Get";

        //route header
        public static string RouteHeaderGetCleanDeliveries = "RouteHeader_GetCleanDeliveries";       
        public static string RouteHeadersGet = "RouteHeaders_Get";
        public static string RoutesCheckDuplicate = "Routes_CheckDuplicate";
        public static string RoutesCreateOrUpdate = "Routes_CreateOrUpdate";
        public static string RoutesGetById = "Routes_GetById";
        

        //Stops
        public static string StopsGetByRouteHeaderId = "Stops_GetByRouteHeaderId";
        
        //Jobs
        public static string JobGetCleanDeliveries = "Job_GetCleanDeliveries";
    }
}
