CREATE PROCEDURE [dbo].[RouteAttributes_GetExceptions]
AS
BEGIN

SELECT
       [ObjectType]
      ,[AttributeName]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[RouteAttributeExceptions]

END