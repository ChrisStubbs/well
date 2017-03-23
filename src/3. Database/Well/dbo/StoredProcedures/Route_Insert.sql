CREATE PROCEDURE [dbo].[Route_Insert]
	@FileName				VARCHAR(255),
	@Username				VARCHAR(50)

AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Routes] ([FileName],[CreatedBy],[DateCreated],[UpdatedBy],[DateUpdated])
	VALUES (@FileName,@Username,GETDATE(),@Username,GETDATE())
	
	SELECT CAST(SCOPE_IDENTITY() as int);
END
