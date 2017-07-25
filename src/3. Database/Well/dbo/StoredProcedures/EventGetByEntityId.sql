CREATE PROCEDURE  [dbo].[EventGetByEntityId]
	@EntityId VARCHAR(50) = NULL,
	@ExceptionActionId int = 0
AS
BEGIN

	SELECT *
  FROM [dbo].[ExceptionEvent]
  WHERE
	EntityId = @EntityId AND ExceptionActionId = @ExceptionActionId
END