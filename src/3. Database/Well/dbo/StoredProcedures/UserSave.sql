CREATE PROCEDURE  [dbo].[UserSave]
	@Name VARCHAR(500),
	@CreatedBy VARCHAR(50),
	@DateCreated DATETIME,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO [dbo].[User]
		([Name],
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated])
	VALUES(
	@Name, @CreatedBy, @DateCreated, @UpdatedBy, @DateUpdated)

	  SELECT CAST(SCOPE_IDENTITY() as int);
END