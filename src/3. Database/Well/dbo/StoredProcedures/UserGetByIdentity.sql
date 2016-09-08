CREATE PROCEDURE  [dbo].[UserGetByIdentity]
	@Identity VARCHAR(255)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		[Id],
		[Name],
		[IdentityName],
		[JobDescription],
		[Domain],
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated],
		[Version]
	  FROM [dbo].[User]
	  WHERE IdentityName = @Identity

END