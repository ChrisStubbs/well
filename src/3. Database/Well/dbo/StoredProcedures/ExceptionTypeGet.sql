CREATE PROCEDURE dbo.ExceptionTypeGet
AS
	SELECT 
		CONVERT(VarChar, id) AS [Key],
		DisplayName AS Value
	FROM 
		ExceptionType
	WHERE
		Id Not IN (0,2)
		-- Ignore Bypass and Not Defined
