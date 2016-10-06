CREATE PROCEDURE [dbo].[Job_CreditLines]
	@Ids dbo.IntTableType	READONLY
AS
BEGIN
	SET NOCOUNT ON;
	Update J 
		SET j.PerformanceStatusId = 8
		FROM  Job AS J
		INNER JOIN @ids ids
		ON ids.Value = j.id		
END
