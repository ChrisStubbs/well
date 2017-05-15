CREATE PROCEDURE [dbo].[JobStatus_Get]
AS
	SELECT 
		CONVERT(VarChar, id) AS [Key],
		[Description] AS Value
	FROM 
		JobStatus