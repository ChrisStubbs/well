CREATE PROCEDURE [dbo].[ExceptionEventDelete]
	@Id int = 0
AS
	DELETE FROM [ExceptionEvent] Where Id  = @Id
