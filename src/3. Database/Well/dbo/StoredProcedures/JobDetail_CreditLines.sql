CREATE PROCEDURE [dbo].[JobDetail_CreditLines]
	@CreditLines dbo.CreditTableType READONLY
AS
BEGIN
	SET NOCOUNT ON;
	Update Jd
		SET JobDetailStatusId = (CASE WHEN creditLines.IsPending = 1 THEN 5 ELSE 1 END)
		FROM  JobDetail AS Jd
		INNER JOIN @CreditLines creditLines
		ON creditLines.CreditId = Jd.JobId	
END
