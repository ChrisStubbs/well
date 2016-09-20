CREATE PROCEDURE [dbo].[Notification_Save]
	@JobId INT,
	@Type TINYINT,
	@Reason VARCHAR(255),
	@CreatedBy VARCHAR(50),
	@DateCreated DATETIME,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
BEGIN

	INSERT INTO [dbo].[Notification](JobId, [Type], Reason, CreatedBy, CreatedDate, LastUpdatedBy, LastUpdatedDate)
	VALUES (@JobId, @Type, @Reason, @CreatedBy, @DateCreated, @UpdatedBy, @DateUpdated)

END
