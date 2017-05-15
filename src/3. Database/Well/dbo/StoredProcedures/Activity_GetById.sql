CREATE PROCEDURE [dbo].[Activity_GetById]
	@Id INT
AS
 BEGIN

	SELECT DocumentNumber
		,InitialDocument
		,ActivityTypeId
		,Id
	FROM Activity
	WHERE Id = @Id 

 RETURN 0
END