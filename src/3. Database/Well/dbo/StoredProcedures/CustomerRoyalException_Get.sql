CREATE PROCEDURE [dbo].[CustomerRoyalException_Get]
AS
BEGIN
SELECT[RoyaltyId]
      ,[Customer]
      ,[ExceptionDays]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[CustomerRoyaltyException]
END