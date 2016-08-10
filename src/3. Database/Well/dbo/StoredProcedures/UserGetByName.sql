CREATE PROCEDURE  [dbo].[UserGetByName]
	@Name VARCHAR(255)
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
	  WHERE Name = @Name OR IdentityName = @Name

END