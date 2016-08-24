CREATE PROCEDURE [dbo].[UserJob_Delete]
	@UserId int,
	@JobId int
AS
BEGIN
	  DELETE FROM [dbo].[UserJob]
	  WHERE UserId = @UserId
	  AND JobId = @JobId
END