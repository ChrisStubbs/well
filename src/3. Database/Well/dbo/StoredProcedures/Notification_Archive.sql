CREATE PROCEDURE [dbo].[Notification_Archive]
	@Id INT,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
BEGIN

	UPDATE [dbo].[Notification]
	SET IsArchived = 1, LastUpdatedBy = @UpdatedBy, LastUpdatedDate = @DateUpdated
	WHERE Id = @Id

END
