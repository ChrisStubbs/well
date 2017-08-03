CREATE PROCEDURE  [dbo].[BranchesGet]
AS
BEGIN

	SET NOCOUNT ON;

	SELECT [Id]
		  ,[Name]
		  ,[CreatedBy]
		  ,[CreatedDate]
		  ,[LastUpdatedBy]
		  ,[LastUpdatedDate]
		  ,[Version]
	  FROM [dbo].[Branch]	  
END