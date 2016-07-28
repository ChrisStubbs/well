CREATE PROCEDURE  [dbo].[UserSave]
	@Name VARCHAR(255),
	@JobDescription VARCHAR(500),
	@IdentityName VARCHAR(255),
	@Domain VARCHAR(50),
	@CreatedBy VARCHAR(50),
	@DateCreated DATETIME,
	@UpdatedBy VARCHAR(50),
	@DateUpdated DATETIME
AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO [dbo].[User]
		([Name],
		[IdentityName],
		[JobDescription],
		[Domain],
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated])
	VALUES(@Name, @IdentityName, @JobDescription, @Domain, @CreatedBy, @DateCreated, @UpdatedBy, @DateUpdated)

	SELECT CAST(SCOPE_IDENTITY() as int);
END