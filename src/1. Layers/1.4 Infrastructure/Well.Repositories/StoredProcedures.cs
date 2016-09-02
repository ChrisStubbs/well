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
        public static string RouteHeaderGetByRouteNumberAndDate = "RouteHeader_GetByRouteNumberAndDate";
        public static string RouteAttributesGetExceptions = "RouteAttributes_GetExceptions";
        public static string RouteHeaderAttributesDeleteByRouteheaderId = "RouteHeaderAttributes_DeleteByRouteheaderId";
        public static string RouteHeaderDeleteById = "RouteHeader_DeleteById";
        public static string RouteHeadersGetForDelete = "RouteHeaders_GetForDelete";
        public static string RoutesGet = "Routes_Get";
        public static string RoutesDeleteById = "Routes_DeleteById";
        public static string RouteheaderGetByRouteId = "Routeheader_GetByRouteId";


        //Stops
        public static string StopsGetByRouteHeaderId = "Stops_GetByRouteHeaderId";
        public static string StopsCreateOrUpdate = "Stops_CreateOrUpdate";
        public static string StopGetById = "Stop_GetById";
        public static string StopAccountCreateOrUpdate = "StopAccount_CreateOrUpdate";
        public static string StopGetByRouteNumberAndDropNumber = "Stop_GetByRouteNumberAndDropNumber";
        public static string StopGeyByOrderUpdateDetails = "Stop_GeyByOrderUpdateDetails";
        public static string StopGetByJobId = "Stop_GetByJobId";
        public static string StopAttributesDeletedByStopId = "StopAttributes_DeletedByStopId";
        public static string StopDeleteById = "Stop_DeleteById";
        public static string AccountDeleteByStopId = "Account_DeleteByStopId";

        //Jobs
        public static string JobCreateOrUpdate = "Job_CreateOrUpdate";
        public static string JobGetById = "Job_GetById";
        public static string JobAttributeCreateOrUpdate = "JobAttribute_CreateOrUpdate";
        public static string JobDeleteById = "Job_DeleteById";

        public static string JobGetByAccountPicklistAndStopId = "Job_GetByAccountPicklistAndStopId";
        public static string JobGetByRefDetails = "Job_GetByRefDetails";
        public static string CustomerRoyalExceptionGet = "CustomerRoyalException_Get";
        public static string JobGetByStopId = "Job_GetByStopId";
        public static string JobArttributesDeleteById = "JobArttributes_DeleteBylId";
        public static string JobGetCreditActionReasons = "Job_GetCreditActionReasons";

        //jobdetail
        public static string JobDetailGet = "JobDetail_Get";
        public static string JobDetailAttributeCreateOrUpdate = "JobDetailAttribute_CreateOrUpdate";
        public static string JobDetailDamageCreateOrUpdate = "JobDetailDamage_CreateOrUpdate";
        public static string JobDetailDeleteById = "JobDetail_DeleteById";
        public static string JobDetailArttributesDeleteByJobDetailId = "JobDetailArttributes_DeleteByJobDetailId";
        public static string JobDetailDeleteDamageReasonsByJobDetailId = "JobDetail_DeleteDamageReasonsByJobDetailId";

        //Account
        public static string AccountGetByStopId = "Account_GetByStopId";
        public static string AccountGetByAccountId = "Account_GetByAccountId";
        public static string AccountGetByAccountCode = "Account_GetByAccountCode";

        //Deliveries
        public static string DeliveriesGetByPerformanceStatus = "Deliveries_GetByPerformanceStatus";
        public static string DeliveryGetById = "Delivery_GetById";
        public static string DeliveryLinesGetByJobId = "DeliveryLines_GetByJobId";

        public static string BranchesGet = "BranchesGet";

        public static string DeleteUserBranches = "DeleteUserBranches";
        public static string SaveUserBranch = "UserBranchInsert";
        public static string GetBranchesForUser = "GetBranchesForUser";

        public static string UsersGetByBranchId = "Users_GetByBranchId";
        public static string UserGetByName = "UserGetByName";
        public static string UserSave = "UserSave";
        public static string AssignJobToUser = "UserJob_Insert";
        public static string UnAssignJobToUser = "UserJob_Delete";

        public static string EventInsert = "EventInsert";
        public static string EventSetProcessed = "EventSetProcessed";
        public static string EventGetUnprocessed = "EventGetUnprocessed";

        public static string MarkEventAsProcessed = "MarkEventAsProcessed";
    }
}
