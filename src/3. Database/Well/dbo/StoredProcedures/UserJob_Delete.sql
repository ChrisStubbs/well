CREATE PROCEDURE [dbo].[UserJob_Delete]
	@JobId int
AS
BEGIN
	  DELETE FROM [dbo].[UserJob]
	  WHERE JobId = @JobId
END