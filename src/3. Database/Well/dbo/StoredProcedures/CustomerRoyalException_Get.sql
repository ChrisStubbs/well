CREATE PROCEDURE [dbo].[CustomerRoyalException_Get]

AS
BEGIN
SELECT [Id]
      ,[Royalty]
      ,[Customer]
  FROM [dbo].[CustomerRoyaltyException]
END