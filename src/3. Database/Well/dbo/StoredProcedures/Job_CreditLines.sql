CREATE PROCEDURE [dbo].[Job_CreditLines]
	@CreditLines dbo.CreditTableType READONLY
AS
BEGIN
	SET NOCOUNT ON;

	Update J 
		SET j.PerformanceStatusId = (CASE WHEN creditLines.IsPending = 1 THEN 9 ELSE 8 END)
		FROM  Job AS J
		INNER JOIN @CreditLines creditLines
		ON creditLines.CreditId = j.id		
END
