CREATE PROCEDURE LineItemActionDelete
	@Id int
AS
	
	DELETE LineItemActionComment
	WHERE LineItemActionId = @Id

	DELETE LineItemAction
	WHERE Id = @Id
