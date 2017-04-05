CREATE PROCEDURE [dbo].[Job_UpdateStatus]
	@Id			int,
	@StatusId	TinyInt
AS
	UPDATE Job
	SET JobStatusId = @StatusId
	WHERE Id = @Id