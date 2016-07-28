CREATE PROCEDURE  [dbo].[GetBranchesForUser]
	@Name VARCHAR(255)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT b.Id, b.Name
	FROM [dbo].[UserBranch] ub
	JOIN [dbo].[User] u on u.Id = ub.UserId
	JOIN [dbo].[Branch] b on b.Id = ub.BranchId
	WHERE u.IdentityName = @Name OR u.IdentityName = @Name

END