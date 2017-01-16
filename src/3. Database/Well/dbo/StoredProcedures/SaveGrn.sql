CREATE PROCEDURE [dbo].[SaveGrn]
	@JobId	INT,
	@Grn	VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE 
		[Job] 
	SET 
		[GrnNumber] = @Grn
	WHERE
		Id = @JobId
END

