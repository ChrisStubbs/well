namespace PH.Well.Repositories
{
    public struct StoredProcedures
    {
        public static string AuditGet = "Audit_Get";
        public static string AuditInsert = "Audit_Insert";

        //route header
        public static string RouteHeadersGet = "RouteHeaders_Get";
        public static string RoutesCheckDuplicate = "Routes_CheckDuplicate";
        public static string RouteInsert = "Route_Insert";
        public static string RoutesGetById = "Routes_GetById";
        public static string RouteHeaderCreateOrUpdate = "RouteHeader_CreateOrUpdate";
        public static string RouteHeaderInsert = "RouteHeader_Insert";
        public static string RouteHeaderUpdate = "RouteHeader_Update";
        public static string RouteHeaderGetById = "RouteHeader_GetById";
        public static string StopAttributeCreateOrUpdate = "StopAttribute_CreateOrUpdate";
        public static string RouteHeaderGetByRouteNumberAndDate = "RouteHeader_GetByRouteNumberAndDate";
        public static string RouteAttributesGetExceptions = "RouteAttributes_GetExceptions";
        public static string RouteHeaderDeleteById = "RouteHeader_DeleteById";
        public static string RouteHeadersGetForDelete = "RouteHeaders_GetForDelete";
        public static string RoutesGet = "Routes_Get";
        public static string RoutesDeleteById = "Routes_DeleteById";
        public static string RouteheaderGetByRouteId = "Routeheader_GetByRouteId";
        public static string RouteheaderGetByNumberDateBranch = "RouteheaderGetByNumberDateBranch";
        public static string HolidayExceptionGet = "HolidayException_Get";

        //Stops
        public static string StopsGetByRouteHeaderId = "Stops_GetByRouteHeaderId";
        public static string StopsCreateOrUpdate = "Stops_CreateOrUpdate";
        public static string StopInsert = "Stop_Insert";
        public static string StopUpdate = "Stop_Update";
        public static string StopGetById = "Stop_GetById";
        public static string StopAccountCreateOrUpdate = "StopAccount_CreateOrUpdate";
        public static string AccountInsert = "Account_Insert";
        public static string StopGetByTransportOrderReference = "StopGetByTransportOrderReference";
        public static string StopGetByOrderUpdateDetails = "Stop_GetByOrderUpdateDetails";
        public static string StopGetByJobId = "Stop_GetByJobId";
        public static string StopDeleteById = "Stop_DeleteById";
        public static string AccountDeleteByStopId = "Account_DeleteByStopId";
        public static string DeleteStopByTransportOrderReference = "DeleteStopByTransportOrderReference";

        //Jobs
        public static string JobCreateOrUpdate = "Job_CreateOrUpdate";
        public static string JobInsert = "Job_Insert";
        public static string JobUpdate = "Job_Update";
        public static string JobGetById = "Job_GetById";
        public static string JobAttributeCreateOrUpdate = "JobAttribute_CreateOrUpdate";
        public static string JobDeleteById = "Job_DeleteById";
        public static string SaveGrn = "SaveGrn";
        public static string JobGetByAccountPicklistAndStopId = "Job_GetByAccountPicklistAndStopId";
        public static string JobGetByRefDetails = "Job_GetByRefDetails";
        public static string CustomerRoyalExceptionGet = "CustomerRoyalException_Get";
        public static string JobGetByStopId = "Job_GetByStopId";
        public static string JobGetCreditActionReasons = "Job_GetCreditActionReasons";
        public static string CustomerRoyaltyExceptionInsert = "CustomerRoyaltyException_Insert";
        public static string CustomerRoyaltyExceptionUpdate = "CustomerRoyaltyException_Update";
        public static string CustomerRoyalExceptionGetByRoyalty = "CustomerRoyalException_GetByRoyalty";
        public static string JobSetToStatus = "Job_SetToStatus";

        //jobdetail
        public static string JobDetailGet = "JobDetail_Get";
        public static string JobDetailInsert = "JobDetail_Insert";
        public static string JobDetailUpdate = "JobDetail_Update";
        public static string JobDetailAttributeCreateOrUpdate = "JobDetailAttribute_CreateOrUpdate";
        public static string JobDetailDeleteById = "JobDetail_DeleteById";
        public static string JobDetailDeleteDamageReasonsByJobDetailId = "JobDetail_DeleteDamageReasonsByJobDetailId";
        public static string JobDetailActionsGet = "JobDetailActionsGet";
        public static string JobDetailDamageGetByJobDetailId = "JobDetailDamage_GetByJobDetailId";
        public static string JobDetailDamageInsert = "JobDetailDamage_Insert";
        public static string JobDetailDamageUpdate = "JobDetailDamage_Update";
        public static string JobDetailDamageDelete = "JobDetailDamage_Delete";
        public static string JobDetailsWithAction = "JobDetails_WithAction";
        public static string JobDetailDamagesWithAction = "JobDetailDamages_WithAction";

        //Account
        public static string AccountGetByStopId = "Account_GetByStopId";
        public static string AccountGetByAccountId = "Account_GetByAccountId";
        public static string AccountGetByAccountCode = "Account_GetByAccountCode";

        //Deliveries
        public static string DeliveriesGetByPerformanceStatus = "Deliveries_GetByPerformanceStatus";
        public static string DeliveriesGetByArrayPerformanceStatus = "Deliveries_GetByArrayPerformanceStatus";
        public static string DeliveryGetById = "Delivery_GetById";
        public static string DeliveryLinesGetByJobId = "DeliveryLines_GetByJobId";
        public static string PendingCreditDeliveriesGet = "PendingCreditDeliveriesGet";

        public static string BranchesGet = "BranchesGet";

        public static string DeleteUserBranches = "DeleteUserBranches";
        public static string SaveUserBranch = "UserBranchInsert";
        public static string GetBranchesForUser = "GetBranchesForUser";
        public static string GetBranchesForSeasonalDate = "GetBranchesForSeasonalDate";
        public static string GetBranchesForCreditThreshold = "GetBranchesForCreditThreshold";
        public static string GetBranchesForCleanPreference = "GetBranchesForCleanPreference";

        public static string UsersGetByBranchId = "Users_GetByBranchId";
        public static string UserGetByIdentity = "UserGetByIdentity";
        public static string UserGetByName = "UserGetByName";
        public static string UserSave = "UserSave";
        public static string AssignJobToUser = "UserJob_Insert";
        public static string UnAssignJobToUser = "UserJob_Delete";
        public static string UserByCreditThresholdGet = "UserByCreditThresholdGet";

        public static string EventInsert = "EventInsert";
        public static string EventSetProcessed = "EventSetProcessed";
        public static string EventGetUnprocessed = "EventGetUnprocessed";

        public static string MarkEventAsProcessed = "MarkEventAsProcessed";

        //Notifications
        public static string SaveNotification = "Notification_Save";
        public static string GetNotifications = "Notifications_Get";
        public static string ArchiveNotification = "Notification_Archive";

        public static string UserStatsGet = "UserStatsGet";
        public static string PendingCreditCountByUserGet = "PendingCreditCountByUserGet";

        public static string SeasonalDatesGetAll = "SeasonalDatesGetAll";
        public static string SeasonalDatesBranchesGet = "SeasonalDatesBranchesGet";
        public static string SeasonalDatesDelete = "SeasonalDatesDelete";
        public static string SeasonalDatesSave = "SeasonalDatesSave";
        public static string SeasonalDatesByBranchGet = "SeasonalDatesByBranchGet";
        public static string SeasonalDatesToBranchSave = "SeasonalDatesToBranchSave";

        public static string CreditThresholdGetAll = "CreditThresholdGetAll";
        public static string CreditThresholdBranchesGet = "CreditThresholdBranchesGet";
        public static string CreditThresholdDelete = "CreditThresholdDelete";
        public static string CreditThresholdByBranch = "CreditThresholdByBranch";
        public static string CreditThresholdSave = "CreditThresholdSave";
        public static string CreditThresholdToBranchSave = "CreditThresholdToBranchSave";

        public static string ThresholdLevelSave = "ThresholdLevelSave";

        public static string CleanPreferencesGetAll = "CleanPreferencesGetAll";
        public static string CleanPreferencesBranchesGet = "CleanPreferencesBranchesGet";
        public static string CleanPreferenceSave = "CleanPreferenceSave";
        public static string CleanPreferenceToBranchSave = "CleanPreferenceToBranchSave";
        public static string CleanPreferenceDelete = "CleanPreferenceDelete";

        public static string RouteIdsToRemoveGet = "RouteIdsToRemoveGet";
        public static string RouteToRemoveFullObjectGraphGet = "RouteToRemoveFullObjectGraphGet";

        public static string CleanPreferenceByBranchGet = "CleanPreferenceByBranchGet";

        public static string WidgetWarningSave = "WidgetWarning_Save";
        public static string WidgetWarningToBranchSave = "WidgetWarningToBranchSave";
        public static string WidgetWarningGetAll = "WidgetWarning_GetAll";
        public static string WidgetWarningBranchesGet = "WidgetWarningBranchesGet";
        public static string WidgetWarningDelete = "WidgetWarningDelete";
        public static string WidgetWarningLevelsByUserGet = "WidgetWarningLevelsByUserGet";

        public static string AssignPendingCreditToUser = "AssignPendingCreditToUser";
        public static string ResolveJobAndJobDetails = "ResolveJobAndJobDetails";
        public static string CreditJob = "Job_CreditLines";
        public static string RemovePendingCredit = "RemovePendingCredit";

        //User
        public static string UserGetCreditThreshold = "User_GetCreditThreshold";

        public static string GetBranchIdForJob = "GetBranchIdForJob";
    }
}
