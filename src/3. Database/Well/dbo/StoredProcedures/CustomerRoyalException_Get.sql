CREATE PROCEDURE [dbo].[CustomerRoyalException_Get]
AS
BEGIN
SELECT
	   [Id]
	  ,[RoyaltyId]
      ,[Customer]
      ,[ExceptionDays]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[CustomerRoyaltyException]
END