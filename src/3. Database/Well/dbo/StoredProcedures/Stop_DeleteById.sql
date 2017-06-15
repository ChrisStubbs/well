CREATE PROCEDURE [dbo].[Stop_DeleteById]
	@Id int
AS

BEGIN
	UPDATE [Stop] 
	SET DateDeleted = GETDATE()
	WHERE Id = @Id
END
