CREATE PROCEDURE [dbo].[Stop_UpdateWellStatus]
	@Id				int,
	@WellStatusId	TinyInt
AS
	UPDATE Stop
	SET WellStatus = @WellStatusId
	WHERE Id = @Id
