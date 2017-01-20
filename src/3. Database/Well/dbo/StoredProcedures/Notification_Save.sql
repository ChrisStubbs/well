CREATE PROCEDURE [dbo].[Notification_Save]
	@JobId INT,
	@Type TINYINT,
	@ErrorMessage VARCHAR(255),
	@CreatedBy VARCHAR(50),
	@DateCreated DATETIME,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
BEGIN

	INSERT INTO [dbo].[Notification](JobId, [Type], ErrorMessage, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate)
	VALUES (@JobId, @Type, @ErrorMessage, @CreatedBy, @DateCreated, @UpdatedBy, @DateUpdated)

END
