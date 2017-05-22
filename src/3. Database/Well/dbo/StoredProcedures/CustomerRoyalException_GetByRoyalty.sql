CREATE PROCEDURE [dbo].[CustomerRoyalException_GetByRoyalty]
	@RoyaltyId int
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
  WHERE [RoyaltyId] = @RoyaltyId
END
