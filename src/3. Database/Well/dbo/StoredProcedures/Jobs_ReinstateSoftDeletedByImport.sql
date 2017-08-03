CREATE PROCEDURE [dbo].[Jobs_ReinstateSoftDeletedByImport]
	@JobIds IntTableType READONLY,
	@UpdatedBy VARCHAR(50)
AS
	
	DECLARE @TableIds TABLE (Id Int, Additional INT NULL)
	-------------------
    --- Reinstate Jobs ---
    -------------------
    UPDATE job
    SET DateDeleted = NULL,
		UpdatedBy	= @UpdatedBy,
		DeletedByImport = 0
    WHERE Job.Id IN (SELECT value FROM @JobIds)
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
    WHERE JobId IN (SELECT value FROM @JobIds) 
	AND DateDeleted IS NOT NULL
	AND DeletedByImport = 1

    ------------------------------
    --- Reinstate JobDetailDamage ---
    ------------------------------
    UPDATE JobDetailDamage 
    SET DateDeleted = NULL,
		UpdatedBy	= @UpdatedBy,
		DeletedByImport = 0
    WHERE JobDetailId IN (SELECT Id FROM @TableIds)
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
