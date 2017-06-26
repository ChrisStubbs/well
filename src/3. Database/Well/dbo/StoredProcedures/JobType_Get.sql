CREATE PROCEDURE [dbo].[JobType_Get]
AS
	SELECT 
		CONVERT(VarChar, id) AS [Key],
		[Description] + ' (' + Abbreviation +')' AS Value
	FROM 
		JobType	
	WHERE
		Code NOT IN ('DEL-DOC', 'UPL-SAN')