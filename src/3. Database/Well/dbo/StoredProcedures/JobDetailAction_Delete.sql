CREATE PROCEDURE [dbo].[JobDetailAction_Delete]
	@JobDetailId	INT,
	@StatusId		INT
AS
BEGIN
	SET NOCOUNT ON;

	Delete from [dbo].[JobDetailAction] WHERE [JobDetailId] = @JobDetailId AND [StatusId] = @StatusId  
END
