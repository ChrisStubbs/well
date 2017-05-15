CREATE PROCEDURE [dbo].[JobType_Get]
AS
	SELECT 
		CONVERT(VarChar, id) AS [Key],
		[Description] AS Value
	FROM 
		JobType