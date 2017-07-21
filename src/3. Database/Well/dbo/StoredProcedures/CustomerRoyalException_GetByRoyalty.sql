CREATE PROCEDURE [dbo].[CustomerRoyalException_GetByRoyalty]
	@RoyaltyCode int
AS
BEGIN
SELECT
	   [Id]
	  ,[RoyaltyCode]
      ,[Customer]
      ,[ExceptionDays]
      ,[CreatedBy]
      ,[DateCreated]
      ,[UpdatedBy]
      ,[DateUpdated]
      ,[Version]
  FROM [dbo].[CustomerRoyaltyException]
  WHERE [RoyaltyCode] = @RoyaltyCode
END
