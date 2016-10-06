CREATE PROCEDURE [dbo].[JobDetail_CreditLines]
	@Ids dbo.IntTableType	READONLY
AS
BEGIN
	SET NOCOUNT ON;
	Update Jd
		SET JobDetailStatusId = 1
		FROM  JobDetail AS Jd
		INNER JOIN @ids ids
		ON ids.Value = Jd.JobId	
END
