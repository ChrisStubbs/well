CREATE PROCEDURE [dbo].[Stop_DeleteById]
	@Id int 
AS
	DELETE FROM Stop WHERE Id = @Id
RETURN 0
