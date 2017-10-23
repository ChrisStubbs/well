CREATE PROCEDURE SyncLineItem
    @JobId Int
AS

	UPDATE LineItem
	SET ProductCode = JobDetail.PHProductCode,
	ProductDescription = JobDetail.ProdDesc
	FROM 
		JobDetail
	WHERE
		JobDetail.LineItemId = LineItem.Id
		AND JobDetail.JobId = @JobId