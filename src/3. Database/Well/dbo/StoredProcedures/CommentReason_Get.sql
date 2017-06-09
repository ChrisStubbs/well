CREATE PROCEDURE [dbo].[CommentReason_Get]
AS
	SELECT 
		CONVERT(VarChar, Id) AS [Key],
		[Description] AS Value
	FROM 
		CommentReason