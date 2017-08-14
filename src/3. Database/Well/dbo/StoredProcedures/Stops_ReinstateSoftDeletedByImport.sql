CREATE PROCEDURE [dbo].[Stops_ReinstateSoftDeletedByImport]
	@StopIds IntTableType READONLY,
	@UpdatedBy VARCHAR(50)
AS
	
	DECLARE @TableIds TABLE (Id Int, Additional INT NULL)
	-------------------
    --- Resinstate Jobs ---
    -------------------
    UPDATE STOP
    SET DateDeleted = NULL,
		UpdatedBy	= @UpdatedBy,
		DeletedByImport = 0
    WHERE STOP.Id IN (SELECT value FROM @StopIds)
	AND DateDeleted IS NOT NULL
	AND DeletedByImport = 1
  
