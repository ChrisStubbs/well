CREATE PROCEDURE [dbo].[JobIdByResolutionStatus_Get]
	@ResolutionStatusId INT
AS
	SELECT 
		Id 
	FROM 
		Job 
	WHERE 
		ResolutionStatusId = @ResolutionStatusId 
		AND DateDeleted is Null


