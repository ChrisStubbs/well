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
        public static string RouteHeaderCreateOrUpdate = "RouteHeader_CreateOrUpdate";
        public static string RouteHeaderGetById = "RouteHeader_GetById";
        public static string RouteHeaderAttributeCreateOrUpdate = "RouteHeaderAttribute_CreateOrUpdate";
        public static string StopAttributeCreateOrUpdate = "StopAttribute_CreateOrUpdate";


        //Stops
        public static string StopsGetByRouteHeaderId = "Stops_GetByRouteHeaderId";
        public static string StopsCreateOrUpdate = "Stops_CreateOrUpdate";
        public static string StopGetById = "Stop_GetById";
        public static string StopAccountCreateOrUpdate = "StopAccount_CreateOrUpdate";

        //Jobs
        public static string JobGetCleanDeliveries = "Job_GetCleanDeliveries";
        public static string JobCreateOrUpdate = "Job_CreateOrUpdate";
        public static string JobGetById = "Job_GetById";
        public static string JobAttributeCreateOrUpdate = "JobAttribute_CreateOrUpdate";
        public static string JobGetByStatus = "Job_GetByStatus";

        //jobdetail
        public static string JobDetailCreateOrUpdate = "JobDetail_CreateOrUpdate";
        public static string JobDetailGetById = "JobDetail_GetById";
        public static string JobDetailAttributeCreateOrUpdate = "JobDetailAttribute_CreateOrUpdate";

        //Account
        public static string AccountGetByStopId = "Account_GetByStopId";



    }
}
