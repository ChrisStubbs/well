﻿CREATE PROCEDURE  [dbo].[Users_Get]
	@UserId INT = null,
	@Identity VARCHAR(255) = null,
	@Name VARCHAR(255) = null,
	@CreditThresholdId INT = null,
	@BranchId INT = NULL
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @UserIdsTable TABLE (Id INT)

	Insert INTO @UserIdsTable
	SELECT Distinct u.[Id] 
	  FROM [dbo].[User] u
	  LEFT JOIN dbo.CreditThresholdUser ctu on ctu.UserId = u.Id
	  LEFT JOIN [dbo].[UserBranch] ub on u.Id = ub.UserId
	  WHERE 
	  (@CreditThresholdId is null or ctu.CreditThresholdId = @CreditThresholdId)
	  AND (@UserId is null OR u.Id = @UserId)
	  AND (@Identity is null or u.[IdentityName] = @Identity)
	  AND (@Name is null or u.[Name] = @Name)
	  AND (@BranchId is null or ub.BranchId = @BranchId)

	SELECT 
		u.[Id],
		u.[Name],
		u.[IdentityName],
		u.[JobDescription],
		u.[Domain],
		ctu.CreditThresholdId as [CreditThresholdId],
		u.[CreatedBy],
		u.[DateCreated],
		u.[UpdatedBy],
		u.[DateUpdated],
		u.[Version]
	  FROM [dbo].[User] u
	  Inner JOIN @UserIdsTable uit on uit.Id = u.Id
	  LEFT JOIN CreditThresholdUser ctu on ctu.UserId = u.Id

	  SELECT
		ct.[Id], 
		ct.[Level], 
		ct.[Threshold], 
		ct.[Description], 
		ct.[CreatedBy], 
		ct.[CreatedDate], 
		ct.[LastUpdatedBy], 
		ct.[LastUpdatedDate]
	FROM
		[dbo].[CreditThreshold] ct
END