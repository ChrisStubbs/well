CREATE PROCEDURE [dbo].[HolidayException_Get]

AS
  SELECT [Id]
      ,[ExceptionDate]
      ,[Exception]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[HolidayExceptions]
  WHERE YEAR([ExceptionDate]) = YEAR(GETDATE())
RETURN 0
