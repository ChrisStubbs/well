CREATE PROCEDURE [dbo].[Job_UpdateWellStatus]
	@Id				int,
	@WellStatusId	TinyInt
AS
	UPDATE Job
	SET WellStatusId = @WellStatusId
	WHERE Id = @Id