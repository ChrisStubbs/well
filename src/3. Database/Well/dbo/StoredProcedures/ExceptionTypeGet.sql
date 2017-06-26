CREATE PROCEDURE dbo.ExceptionTypeGet
AS
	SELECT 
		CONVERT(VarChar, id) AS [Key],
		DisplayName AS Value
	FROM 
		ExceptionType
	WHERE
		Id != 2 -- BYPASS Disable for now

