CREATE PROCEDURE [dbo].[ExceptionAction_Get]
AS
	SELECT 
		CONVERT(VarChar, id) AS [Key],
		Description AS Value
	FROM 
		ExceptionAction