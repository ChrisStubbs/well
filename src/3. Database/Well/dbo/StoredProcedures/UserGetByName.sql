CREATE PROCEDURE  [dbo].[UserGetByName]
	@Name VARCHAR(500)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		[Id],
		[Name],
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated],
		[Version]
	  FROM [dbo].[User]
	  WHERE Name = @Name

END