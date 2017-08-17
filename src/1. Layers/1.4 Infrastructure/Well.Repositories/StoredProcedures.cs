using System;

namespace PH.Well.Repositories
{
    public struct StoredProcedures
    {
        public const string RouteHeaderGetAll = "RouteHeader_GetAll";
        public const string RoutesCheckDuplicate = "Routes_CheckDuplicate";
        public const string RouteInsert = "Route_Insert";
        public const string RoutesGetById = "Routes_GetById";
        public const string RouteHeaderCreateOrUpdate = "RouteHeader_CreateOrUpdate";
        public const string RouteHeaderInsert = "RouteHeader_Insert";
        public const string RouteHeaderUpdate = "RouteHeader_Update";
        public const string RouteHeaderGetById = "RouteHeader_GetById";
        public const string StopAttributeCreateOrUpdate = "StopAttribute_CreateOrUpdate";
        public const string RouteHeaderGetByBranchRouteNumberAndDate = "RouteHeader_GetByBranchRouteNumberAndDate";
        public const string RouteAttributesGetExceptions = "RouteAttributes_GetExceptions";
        public const string RouteHeaderDeleteById = "RouteHeader_DeleteById";
        public const string RoutesGet = "Routes_Get";
        public const string RoutesDeleteById = "Routes_DeleteById";
        public const string RouteheaderGetByRouteId = "Routeheader_GetByRouteId";
        public const string RouteheaderGetByNumberDateBranch = "RouteheaderGetByNumberDateBranch";
        public const string HolidayExceptionGet = "HolidayException_Get";
        public const string StopsGetByRouteHeaderId = "Stops_GetByRouteHeaderId";
        public const string StopsCreateOrUpdate = "Stops_CreateOrUpdate";
        public const string StopInsert = "Stop_Insert";
        public const string StopUpdate = "Stop_Update";
        public const string StopAccountCreateOrUpdate = "StopAccount_CreateOrUpdate";
        public const string AccountInsert = "Account_Insert";
        public const string AccountUpdate = "Account_Update";
        public const string StopGetByJob = "StopGetByJob";
        public const string StopIdsGetByTransportOrderReference = "StopIds_GetByTransportOrderReference";
        public const string StopGetByJobId = "Stop_GetByJobId";
        public const string StopsGetByIds = "Stops_GetByIds";
        public const string StopDeleteById = "Stop_DeleteById";
        public const string AccountDeleteByStopId = "Account_DeleteByStopId";
        public const string DeleteStopByTransportOrderReference = "DeleteStopByTransportOrderReference";
        public const string JobCreateOrUpdate = "Job_CreateOrUpdate";
        public const string JobInsert = "Job_Insert";
        public const string JobUpdateStatus = "Job_UpdateStatus";
        public const string JobUpdate = "Job_Update";
        public const string JobGetById = "Job_GetById";
        public const string JobAttributeCreateOrUpdate = "JobAttribute_CreateOrUpdate";
        public const string JobDeleteById = "Job_DeleteById";
        public const string JobsCascadeSoftDelete = "Jobs_CascadeSoftDelete";
        public const string JobsReinstateSoftDeletedByImport = "Jobs_ReinstateSoftDeletedByImport";
        public const string StopsReinstateSoftDeletedByImport = "Stops_ReinstateSoftDeletedByImport";
        public const string SaveGrn = "SaveGrn";
        public const string JobGetByRefDetails = "Job_GetByRefDetails";
        public const string CustomerRoyalExceptionGet = "CustomerRoyalException_Get";
        public const string JobGetByStopId = "Job_GetByStopId";

        [Obsolete("Replaced with EF version. Should be deleted")]
        public const string JobDetailTotalsPerStop = "JobDetailTotalsPerStop";
        [Obsolete("Replaced with EF version. Should be deleted")]
        public const string JobDetailTotalsPerRouteHeader = "JobDetailTotalsPerRouteHeader";
        [Obsolete("Replaced with EF version. Should be deleted")]
        public const string JobDetailTotalsPerJobIds = "JobDetailTotalsPerJobIds";
        public const string JobGetCreditActionReasons = "Job_GetCreditActionReasons";
        public const string CustomerRoyaltyExceptionInsert = "CustomerRoyaltyException_Insert";
        public const string CustomerRoyaltyExceptionUpdate = "CustomerRoyaltyException_Update";
        public const string CustomerRoyalExceptionGetByRoyalty = "CustomerRoyalException_GetByRoyalty";
        public const string JobSetToStatus = "Job_SetToStatus";
        public const string JobGetByBranchAndInvoiceNumberWithFullObjectGraph = "Job_GetByBranchAndInvoiceNumber";
        public const string JobGetByIds = "Job_GetByIds";
        public const string JobGetByRouteHeaderId = "Job_GetByRouteHeaderId";
        public const string GetJobIdsByBranchAccountPickListRefAndJobType = "JobIds_GetByBranchAccountPickListRefAndJobTypeIncludeSoftDeleted";
        public const string JobDetailGet = "JobDetail_Get";
        public const string JobDetailInsert = "JobDetail_Insert";
        public const string JobDetailUpdate = "JobDetail_Update";
        public const string JobDetailAttributeCreateOrUpdate = "JobDetailAttribute_CreateOrUpdate";
        public const string JobDetailDeleteById = "JobDetail_DeleteById";
        public const string JobDetailDeleteDamageReasonsByJobDetailId = "JobDetail_DeleteDamageReasonsByJobDetailId";
        public const string JobDetailDamageGetByJobDetailId = "JobDetailDamage_GetByJobDetailId";
        public const string JobDetailDamageInsert = "JobDetailDamage_Insert";
        public const string JobDetailDamageUpdate = "JobDetailDamage_Update";
        public const string JobDetailDamageDelete = "JobDetailDamage_Delete";
        public const string AccountGetByStopId = "Account_GetByStopId";
        public const string AccountGetByAccountId = "Account_GetByAccountId";
        public const string DeliveriesGetByPerformanceStatus = "Deliveries_GetByPerformanceStatus";
        public const string DeliveriesGetByArrayPerformanceStatus = "Deliveries_GetByArrayPerformanceStatus";
        public const string DeliveryGetById = "Delivery_GetById";
        public const string DeliveryLinesGetByJobId = "DeliveryLines_GetByJobId";
        public const string DeliveriesGet = "Deliveries_Get";
        public const string BranchesGet = "BranchesGet";
        public const string DeleteUserBranches = "DeleteUserBranches";
        public const string SaveUserBranch = "UserBranchInsert";
        public const string GetBranchesForUser = "GetBranchesForUser";
        public const string GetBranchesForSeasonalDate = "GetBranchesForSeasonalDate";
        public const string GetBranchesForCleanPreference = "GetBranchesForCleanPreference";
        public const string CleanJobsSetResolutionStatusClosed = "CleanJobsSetResolutionStatusClosed";
        public const string CleanStops = "CleanStops";
        public const string CleanRoutes = "CleanRoutes";

        public const string UsersGet = "Users_Get";
        public const string UserSave = "UserSave";
        public const string AssignJobToUser = "UserJob_Insert";
        public const string UnAssignJobToUser = "UserJob_Delete";
        public const string EventInsert = "EventInsert";
        public const string EventInsertBulk = "EventInsertBulk";
        
        public const string EventSetProcessed = "EventSetProcessed";
        public const string EventGetUnprocessed = "EventGetUnprocessed";
        public const string MarkEventAsProcessed = "MarkEventAsProcessed";
        public const string SaveNotification = "Notification_Save";
        public const string GetNotifications = "Notifications_Get";
        public const string ArchiveNotification = "Notification_Archive";
        public const string SeasonalDatesGetAll = "SeasonalDatesGetAll";
        public const string SeasonalDatesBranchesGet = "SeasonalDatesBranchesGet";
        public const string SeasonalDatesDelete = "SeasonalDatesDelete";
        public const string SeasonalDatesSave = "SeasonalDatesSave";
        public const string SeasonalDatesByBranchGet = "SeasonalDatesByBranchGet";
        public const string SeasonalDatesToBranchSave = "SeasonalDatesToBranchSave";
        public const string CreditThresholdGetAll = "CreditThresholdGetAll";
        public const string CreditThresholdDelete = "CreditThresholdDelete";
        public const string CreditThresholdSave = "CreditThresholdSave";
        public const string CreditThresholdUpdate = "CreditThresholdUpdate";
        public const string CreditThresholdGetByUser = "CreditThresholdGetByUser";
        public const string CreditThresholdUserDelete = "CreditThresholdUserDelete";
        public const string CreditThresholdUserInsert = "CreditThresholdUserInsert";


        public const string RouteIdsToRemoveGet = "RouteIdsToRemoveGet";
        public const string RouteToRemoveFullObjectGraphGet = "RouteToRemoveFullObjectGraphGet";
        public const string WidgetWarningSave = "WidgetWarning_Save";
        public const string WidgetWarningToBranchSave = "WidgetWarningToBranchSave";
        public const string WidgetWarningGetAll = "WidgetWarning_GetAll";
        public const string WidgetWarningBranchesGet = "WidgetWarningBranchesGet";
        public const string WidgetWarningDelete = "WidgetWarningDelete";
        public const string WidgetWarningLevelsByUserGet = "WidgetWarningLevelsByUserGet";
        public const string PendingCreditInsert = "PendingCreditInsert";
        public const string CreditJob = "Job_CreditLines";
        public const string RemovePendingCredit = "RemovePendingCredit";
        public const string GetUserJobsByJobIds = "UserJobs_GetbyJobIds";
        public const string GetBranchIdForJob = "GetBranchIdForJob";
        public const string GetBranchIdForStop = "GetBranchIdForStop";

        public const string RoutesGetAllForBranch = "Routes_GetAllForBranch";
        public const string RoutesGetByIds = "Routes_GetByIds";

        public static string AppSearch = "AppSearch";
        //well update
        public const string JobDetailTobaccoUpdate = "JobDetail_TobaccoUpdate";
        public const string JobUpdateShortsTba = "Job_UpdateShortsTBA";
        public const string PostImportUpdate = "PostImportUpdate";
        public const string LineItemActionInsert = "LineItemAction_Insert";

        public const string AssigneeGetByRouteHeaderId = "Assignee_GetByRouteHeaderId";
        public const string AssigneeGetByStopId = "Assignee_GetByStopId";
        public const string AssigneeGetByJobIds = "Assignee_GetByJobIds";

        public const string LineItemGetByIds = "LineItem_GetByIds";
        public const string LineItemGetByActivityId = "LineItem_GetByActivityId";
        public const string LineItemActionGetByLineItemId = "LineItemAction_GetByLineItemId";
        public const string LineItemActionGetByLineItemIds = "LineItemAction_GetByLineItemIds";
        public const string LineItemIdsGetByJobIds = "LineItemIds_GetByJobIds";

        //Line Item Actions
        public const string LineItemActionInsertByUser = "LineItemAction_InsertByUser";
        public const string LineItemActionGetByIds = "LineItemActionGetByIds";
        public const string LineItemActionUpdate = "LineItemActionUpdate";
        public const string LineItemActionSubmitModelGetUnsubmitted = "LineItemActionSubmitModelGetUnsubmitted";
        public const string LineItemActionDeleteForJob = "LineItemAction_DeleteForJob";

        public const string ActivityGetById = "Activity_GetById";
        public const string LocationGetById = "Location_GetById";
        public const string GetSingleLocation = "SingleLocation_Get";
        public const string GetLocations = "Locations_GET";
        
        //lookups
        public const string ExceptionType = "ExceptionTypeGet";
        public const string ExceptionAction = "ExceptionAction_Get";
        public const string JobStatus = "JobStatus_Get";
        public const string JobType = "JobType_Get";
        public const string Driver = "Drivers_Get";
        public const string CommentReason = "CommentReason_Get";

        public const string LineItemActionCommentInsert = "LineItemActionCommentInsert";
        public const string LineItemActionCommentUpdate = "LineItemActionCommentUpdate";
        public const string GetJobRoutesByJobIds = "JobRoutes_GetByJobIds";
        public const string JobResolutionStatusInsert = "JobResolutionStatus_Insert";
        public const string JobsToBeApproved = "JobsToBeApproved";
        public const string GetJobIdsByLineItemIds = "JobIds_GetByLineItemIds";
        public const string GetJobIdsByStopIds = "JobIds_GetByStopIds";
        public const string ActivityGetByDocumentNumber = "Activity_GetByDocumentNumber";

        public const string JobGetWithLineItemActions = "Job_GetWithLineItemActions";

        public const string GetAmendments = "Amendment_GetByJobIds";

        //DateThreshold
        public const string DateThreshold = "DateThreshold_Get";
        public const string DateThresholdDelete = "DateThreshold_Delete";
        public const string DateThresholdUpdate = "DateThreshold_Update";

        public const string CleanPreferenceDelete = "CleanPreferenceDelete";
        
    }
}
