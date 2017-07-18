CREATE PROCEDURE [dbo].[LineItemAction_DeleteForJob]
	@JobId INT,
	@UpdatedBy VARCHAR(50),
	@UpdatedDate DATETIME
AS
BEGIN
	SET NOCOUNT ON

	UPDATE lia
	SET	
		LastUpdatedBy = @UpdatedBy,
		LastUpdatedDate = @UpdatedDate,
		DateDeleted = @UpdatedDate
	FROM 
		LineItemAction lia
	INNER JOIN JobDetail jd on lia.LineItemId = jd.LineItemId
	WHERE jd.JobId = @JobId

END
		
