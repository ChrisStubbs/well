CREATE PROCEDURE  [dbo].[DeleteUserBranches]
	@UserId int
AS
BEGIN

	DELETE FROM [dbo].[UserBranch] WHERE UserId = @UserId

END