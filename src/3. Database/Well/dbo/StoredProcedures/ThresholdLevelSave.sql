CREATE PROCEDURE  [dbo].[ThresholdLevelSave]
	@ThresholdLevelId INT,
	@UserId INT
AS
BEGIN
	UPDATE [dbo].[User] SET [ThresholdLevelId] = @ThresholdLevelId WHERE Id = @UserId
END