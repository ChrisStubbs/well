
CREATE PROCEDURE dbo.GetExceptionType
AS
	SELECT 
		CONVERT(VarChar, id) AS [Key],
		DisplayName AS Value
	FROM 
		ExceptionType