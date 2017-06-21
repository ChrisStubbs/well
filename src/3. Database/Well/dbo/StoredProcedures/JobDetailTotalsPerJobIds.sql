CREATE PROCEDURE [dbo].[JobDetailTotalsPerJobIds]
	@JobIds dbo.IntTableType	READONLY
AS
		SELECT 
		lie.BypassTotal,
		lie.DamageTotal,
		lie.ShortTotal,
		lie.TotalExceptions,
		jb.Id AS JobDetailId
	FROM 
		LineItemExceptionsView lie
		INNER JOIN LineItem li
			ON lie.Id = li.Id
			AND li.DateDeleted IS NULL
		INNER JOIN JobDetail jb
			ON li.Id = jb.LineItemId
		INNER JOIN @JobIds ids ON ids.Value = jb.JobId
		
RETURN 0
