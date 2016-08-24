﻿namespace PH.Well.Repositories
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
        public static string RouteHeaderGetByRouteNumberAndDate = "RouteHeader_GetByRouteNumberAndDate";
        public static string RouteAttributesGetExceptions = "RouteAttributes_GetExceptions";


        //Stops
        public static string StopsGetByRouteHeaderId = "Stops_GetByRouteHeaderId";
        public static string StopsCreateOrUpdate = "Stops_CreateOrUpdate";
        public static string StopGetById = "Stop_GetById";
        public static string StopAccountCreateOrUpdate = "StopAccount_CreateOrUpdate";
        public static string StopGetByRouteNumberAndDropNumber = "Stop_GetByRouteNumberAndDropNumber";

        //Jobs
        public static string JobCreateOrUpdate = "Job_CreateOrUpdate";
        public static string JobGetById = "Job_GetById";
        public static string JobAttributeCreateOrUpdate = "JobAttribute_CreateOrUpdate";
        
        public static string JobGetByAccountPicklistAndStopId = "Job_GetByAccountPicklistAndStopId";

        //jobdetail
        public static string JobDetailCreateOrUpdate = "JobDetail_CreateOrUpdate";
        public static string JobDetailGetById = "JobDetail_GetById";
        public static string JobDetailAttributeCreateOrUpdate = "JobDetailAttribute_CreateOrUpdate";
        public static string JobDetailGetByBarcodeLineNumberAndJobId = "JobDetail_GetByBarcodeLineNumberAndJobId";
        public static string JobDetailDamageCreateOrUpdate = "JobDetailDamage_CreateOrUpdate";

        //Account
        public static string AccountGetByStopId = "Account_GetByStopId";
        public static string AccountGetByAccountId = "Account_GetByAccountId";

        //Deliveries
        public static string DeliveriesGetByPerformanceStatus = "Deliveries_GetByPerformanceStatus";
        public static string DeliveryGetById = "Delivery_GetById";
        public static string DeliveryLinesGetById = "DeliveryLines_GetById";

        public static string BranchesGet = "BranchesGet";

        public static string DeleteUserBranches = "DeleteUserBranches";
        public static string SaveUserBranch = "UserBranchInsert";
        public static string GetBranchesForUser = "GetBranchesForUser";

        public static string UsersGetByBranchId = "Users_GetByBranchId";
        public static string UserGetByName = "UserGetByName";
        public static string UserSave = "UserSave";
        public static string AssignJobToUser = "UserJob_Insert";

        public static string EventInsert = "EventInsert";
        public static string EventSetProcessed = "EventSetProcessed";
        public static string EventGetUnprocessed = "EventGetUnprocessed";

        public static string MarkEventAsProcessed = "MarkEventAsProcessed";
    }
}
