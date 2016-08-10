CREATE PROCEDURE  [dbo].[EventGetUnprocessed]
AS
BEGIN

	SELECT 
	   [Id]
      ,[Event]
      ,[ExceptionActionId]
      ,[Processed]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[ExceptionEvent]
  WHERE
	Processed = 0
END