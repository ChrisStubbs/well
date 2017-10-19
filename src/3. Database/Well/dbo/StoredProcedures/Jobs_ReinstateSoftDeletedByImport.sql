CREATE PROCEDURE [dbo].[Jobs_ReinstateSoftDeletedByImport]
	@BranchId AS INT,
	@IdentifyJobTable dbo.IdentifyJobTableType	READONLY,
	@UpdatedBy VARCHAR(50)
AS
	IF OBJECT_ID('tempdb..#JobIds') IS NOT NULL 
		DROP TABLE #JobIds

	SELECT 
		j.Id AS JobId, 
		s.Id AS StopId, 
		j.PhAccount, 
		j.PickListRef, 
		j.JobTypeCode, 
		j.WellStatusId, 
		j.JobStatusId, 
		j.GrnNumber, 
		j.RoyaltyCode,
		rh.RouteOwnerId AS BranchId,
		rh.RouteDate,
        j.ResolutionStatusId
	INTO #JobIds
	FROM 
		Job j
		INNER JOIN Stop s 
			on s.id = j.StopId
		INNER JOIN RouteHeader rh 
			on s.RouteHeaderId = rh.Id
		INNER JOIN @IdentifyJobTable idt 
			ON idt.PHAccount = j.PHAccount and idt.PicklistRef = j.PickListRef 
			and idt.JobTypeCode = j.JobTypeCode
	WHERE 
		rh.RouteOwnerId = @BranchId


	DECLARE @TableIds TABLE (Id Int, Additional INT NULL)
	-------------------
    --- Reinstate Jobs ---
    -------------------
    UPDATE job
    SET DateDeleted = NULL,
		UpdatedBy	= @UpdatedBy,
		DeletedByImport = 0
    WHERE 
		Job.Id IN (SELECT JobId FROM #JobIds)
		AND DateDeleted IS NOT NULL
		AND DeletedByImport = 1
    ------------------------
    --- Reinstate JobDetail ---
    ------------------------
    UPDATE JobDetail 
    SET DateDeleted = NULL,
		UpdatedBy	= @UpdatedBy,
		DeletedByImport = 0
    OUTPUT inserted.Id, inserted.LineItemId INTO @TableIds
    WHERE 
		JobId IN (SELECT JobId FROM #JobIds) 
		AND DateDeleted IS NOT NULL
		AND DeletedByImport = 1

    ------------------------------
    --- Reinstate JobDetailDamage ---
    ------------------------------
    UPDATE JobDetailDamage 
    SET DateDeleted = NULL,
		UpdatedBy	= @UpdatedBy,
		DeletedByImport = 0
    WHERE 
		JobDetailId IN (SELECT Id FROM @TableIds)
		AND DateDeleted IS NOT NULL
		AND DeletedByImport = 1

    --reset the stored ids
    delete @TableIds

    -----------------------
    --- Reinstate LineItem ---
    -----------------------
    UPDATE LineItem
    SET DateDeleted = NULL,
		LastUpdatedBy	= @UpdatedBy,
		DeletedByImport = 0
    OUTPUT inserted.Id, NULL INTO @TableIds
    WHERE Id IN (SELECT Additional FROM @TableIds)
	AND DateDeleted IS NOT NULL
	AND DeletedByImport = 1

    -----------------------------
    --- Reinstate LineItemAction ---
    -----------------------------
    UPDATE LineItemAction
    SET DateDeleted = NULL,
		LastUpdatedBy	= @UpdatedBy,
		DeletedByImport = 0
    OUTPUT inserted.Id, inserted.Id INTO @TableIds
    WHERE LineItemId IN (SELECT Id FROM @TableIds)
	AND DateDeleted IS NOT NULL
	AND DeletedByImport = 1

    ------------------------------------
    --- Reinstate LineItemActionComment ---
    ------------------------------------
    UPDATE LineItemActionComment
    SET DateDeleted = NULL,
		UpdatedBy	= @UpdatedBy,
		DeletedByImport = 0
    WHERE LineItemActionId IN (SELECT Additional FROM @TableIds)
	AND DateDeleted IS NOT NULL
	AND DeletedByImport = 1

	SELECT 
        j.JobId, 
		j.StopId, 
		j.PhAccount, 
		j.PickListRef, 
		j.JobTypeCode, 
		j.WellStatusId, 
		j.JobStatusId, 
		j.GrnNumber, 
		j.RoyaltyCode,
		j.BranchId,
		j.RouteDate,
        j.ResolutionStatusId
	FROM #JobIds j

	IF OBJECT_ID('tempdb..#JobIds') IS NOT NULL 
		DROP TABLE #JobIds