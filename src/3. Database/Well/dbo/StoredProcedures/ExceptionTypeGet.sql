CREATE PROCEDURE dbo.ExceptionTypeGet
AS
	SELECT 
		CONVERT(VarChar, id) AS [Key],
		DisplayName AS Value
	FROM 
		ExceptionType