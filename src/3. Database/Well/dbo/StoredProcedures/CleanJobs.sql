CREATE PROCEDURE CleanJobs
    @JobIds IntTableType READONLY,
    @DateDeleted DateTime
AS
    DECLARE @TableIds TABLE (Id Int, AdicionalId INT NULL)

    /************************************************/
    /*** Set resolution status close to open jobs ***/
    /************************************************/
    UPDATE JOB
    SET ResolutionStatusId = ISNULL(NULLIF(ResolutionStatusId, 1), 2) | 256 --if job is status = imported set it to driver completed 
    WHERE 
        Job.Id IN (SELECT value FROM @JobIds)
        AND job.ResolutionStatusId IN (1, 2, 512)

    -------------------
    --- Delete Jobs ---
    -------------------
    UPDATE job
    SET DateDeleted = @DateDeleted
    WHERE Job.Id IN (SELECT value FROM @JobIds)

    ------------------------
    --- Delete JobDetail ---
    ------------------------
    UPDATE JobDetail 
    SET DateDeleted = @DateDeleted
    OUTPUT inserted.Id, inserted.LineItemId INTO @TableIds
    WHERE JobId IN (SELECT value FROM @JobIds) 

    ------------------------------
    --- Delete JobDetailDamage ---
    ------------------------------
    UPDATE JobDetailDamage 
    SET DateDeleted = @DateDeleted
    WHERE JobDetailId IN (SELECT Id FROM @TableIds)

    --reset the stored ids
    delete @TableIds

    -----------------------
    --- Delete LineItem ---
    -----------------------
    UPDATE LineItem
    SET DateDeleted = @DateDeleted
    OUTPUT inserted.Id, NULL INTO @TableIds
    WHERE Id IN (SELECT AdicionalId FROM @TableIds)

    -----------------------------
    --- Delete LineItemAction ---
    -----------------------------
    UPDATE LineItemAction
    SET DateDeleted = @DateDeleted
    OUTPUT inserted.Id, inserted.Id INTO @TableIds
    WHERE LineItemId IN (SELECT Id FROM @TableIds)

    ------------------------------------
    --- Delete LineItemActionComment ---
    ------------------------------------
    UPDATE LineItemActionComment
    SET DateDeleted = @DateDeleted
    WHERE LineItemActionId IN (SELECT AdicionalId FROM @TableIds)