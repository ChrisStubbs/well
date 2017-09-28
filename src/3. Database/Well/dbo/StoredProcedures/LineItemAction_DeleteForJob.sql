CREATE PROCEDURE [dbo].[LineItemAction_DeleteForJob]
	@JobId INT
AS
BEGIN
	SET NOCOUNT ON

	DELETE liac
	FROM
		LineItemActionComment liac
	INNER JOIN
		LineItemAction lia On lia.id =  liac.LineItemActionId
	INNER JOIN 
		JobDetail jd on lia.LineItemId = jd.LineItemId
	WHERE jd.JobId = @JobId

	DELETE lia
	FROM 
		LineItemAction lia
	INNER JOIN 
		JobDetail jd on lia.LineItemId = jd.LineItemId
	WHERE 
		jd.JobId = @JobId

END
		
