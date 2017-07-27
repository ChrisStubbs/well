CREATE PROCEDURE  EventInsertBulk
	@Data EventTableType READONLY
AS
	INSERT INTO ExceptionEvent([Event], ExceptionActionId, Processed, DateCanBeProcessed, CreatedBy, DateCreated, UpdatedBy, DateUpdated)
    SELECT 
        d.Event, d.ExceptionActionId, 0 AS Processed, d.DateCanBeProcessed, d.CreatedBy, d.DateCreated, d.UpdatedBy, d.DateUpdated
    FROM 
        @Data d