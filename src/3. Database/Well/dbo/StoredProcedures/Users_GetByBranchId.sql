CREATE PROCEDURE  [dbo].[Users_GetByBranchId]
	@BranchId INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT u.[Id]
		  ,u.[Name]
		  ,u.[IdentityName]
		  ,u.[JobDescription]
		  ,u.[Domain]
		  ,u.[CreatedBy]
		  ,u.[DateCreated]
		  ,u.[UpdatedBy]
		  ,u.[DateUpdated]
		  ,u.[Version]
	  FROM [dbo].[User] u
	  INNER JOIN [dbo].[UserBranch] ub on u.Id = ub.UserId
	  WHERE ub.BranchId = @BranchId

END