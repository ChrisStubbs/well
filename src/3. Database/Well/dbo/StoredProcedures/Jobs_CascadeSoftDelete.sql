CREATE PROCEDURE [dbo].[Jobs_CascadeSoftDelete]
	@JobIds IntTableType READONLY,
    @DateDeleted DateTime,
	@UpdatedBy VARCHAR(50),
	@DeletedByImport BIT = 0
AS
	
	DECLARE @TableIds TABLE (Id Int, Additional INT NULL)
	-------------------
    --- Delete Jobs ---
    -------------------
    UPDATE Job
    SET DateDeleted = @DateDeleted
		,UpdatedBy	= @UpdatedBy
		,DeletedByImport = @DeletedByImport
    WHERE Job.Id IN (SELECT value FROM @JobIds)

    ------------------------
    --- Delete JobDetail ---
    ------------------------
    UPDATE JobDetail 
    SET DateDeleted = @DateDeleted
		,UpdatedBy	= @UpdatedBy
		,DeletedByImport = @DeletedByImport
    OUTPUT inserted.Id, inserted.LineItemId INTO @TableIds
    WHERE JobId IN (SELECT value FROM @JobIds) 

    ------------------------------
    --- Delete JobDetailDamage ---
    ------------------------------
    UPDATE JobDetailDamage 
    SET DateDeleted = @DateDeleted
		,UpdatedBy	= @UpdatedBy
		,DeletedByImport = @DeletedByImport
    WHERE JobDetailId IN (SELECT Id FROM @TableIds)

    --reset the stored ids
    delete @TableIds

    -----------------------
    --- Delete LineItem ---
    -----------------------
    UPDATE LineItem
    SET DateDeleted = @DateDeleted
		,LastUpdatedBy	= @UpdatedBy
		,DeletedByImport = @DeletedByImport
    OUTPUT inserted.Id, NULL INTO @TableIds
    WHERE Id IN (SELECT Additional FROM @TableIds)

    -----------------------------
    --- Delete LineItemAction ---
    -----------------------------
    UPDATE LineItemAction
    SET DateDeleted = @DateDeleted
		,DeletedByImport = @DeletedByImport
		,LastUpdatedBy	= @UpdatedBy
    OUTPUT inserted.Id, inserted.Id INTO @TableIds
    WHERE LineItemId IN (SELECT Id FROM @TableIds)

    ------------------------------------
    --- Delete LineItemActionComment ---
    ------------------------------------
    UPDATE LineItemActionComment
    SET DateDeleted = @DateDeleted
		,DeletedByImport = @DeletedByImport
		,UpdatedBy	= @UpdatedBy
    WHERE LineItemActionId IN (SELECT Additional FROM @TableIds)

	------------------
	--- Delete Bag ---
	------------------
	UPDATE Bag
	SET DateDeleted = @DateDeleted
		,LastUpdatedBy	= @UpdatedBy
	WHERE Id IN (SELECT BagId FROM LineItem l JOIN @TableIds i ON l.Id = i.Id)