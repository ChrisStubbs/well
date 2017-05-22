CREATE PROCEDURE  [dbo].[Account_GetByAccountId]
	@AccountId INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT [Code]
		  ,[Name]
		  ,[Address1]
		  ,[Address2]
		  ,[PostCode]
		  ,[ContactName]
		  ,[ContactNumber]
		  ,[ContactNumber2]
		  ,[ContactEmailAddress]
		  ,[DepotId]
	  FROM [dbo].[Account]
	  WHERE Id = @AccountId

END