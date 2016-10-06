CREATE PROCEDURE [dbo].[Stop_DeleteById]
	@Id int
AS

BEGIN
	UPDATE [Stop] 
	SET IsDeleted = 1
	WHERE Id = @Id
END
