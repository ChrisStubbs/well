CREATE PROCEDURE [dbo].[ExceptionEvent_GetBySourceId]
	@ExceptionActionId int,
	@SourceId varchar(50)
	
AS
	SELECT 
	   [Id]
      ,[Event]
      ,[ExceptionActionId]
      ,[Processed]
      ,[DateCanBeProcessed]
      ,[SourceId]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
  FROM 
	[dbo].[ExceptionEvent] 
  WHERE
	ExceptionActionId = @ExceptionActionId
	AND SourceId = @SourceId

