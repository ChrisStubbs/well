CREATE PROCEDURE  [dbo].[GetBranchesForUser]
	@Username VARCHAR(500)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT b.Id, b.Name
	FROM [dbo].[UserBranch] ub
	JOIN [dbo].[User] u on u.Id = ub.UserId
	JOIN [dbo].[Branch] b on b.Id = ub.BranchId
	WHERE u.Name = @Username

END