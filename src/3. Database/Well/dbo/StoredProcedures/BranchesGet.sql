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
	  
	  SELECT  ct.Id,
			  ct.Value,
			  tl.Id as ThresholdLevelId,
			  ctb.BranchId
		from [dbo].[CreditThresholdToBranch] ctb
		inner join [dbo].[CreditThreshold] ct on ct.Id = ctb.CreditThresholdId
		inner join [dbo].[ThresholdLevel] tl on ct.ThresholdLevelId = tl.Id

END