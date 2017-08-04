CREATE PROCEDURE  [dbo].[Account_GetByStopId]
	@StopId INT
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
		  ,[LocationId]
	  FROM [dbo].[Account]
	  WHERE StopId = @StopId

END