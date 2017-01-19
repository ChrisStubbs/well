CREATE PROCEDURE [dbo].[JobDetails_WithAction]
	@jobId INT,
	@action TINYINT
AS
BEGIN
	SELECT jd.JobId AS Id,
		jd.PHProductCode AS ProductCode,
		jda.ActionId AS [Action],
		jd.JobDetailReasonId AS Reason,
		jd.JobDetailSourceId AS [Source],
		jda.Quantity
	FROM 
		dbo.JobDetail jd
	JOIN
		dbo.JobDetailAction jda on jda.JobDetailId = jd.Id
	JOIN 
		dbo.ExceptionAction ea on ea.Id = jda.ActionId
	WHERE 
		jd.JobId = @jobId
	AND
		ea.Id = @action
	
END
