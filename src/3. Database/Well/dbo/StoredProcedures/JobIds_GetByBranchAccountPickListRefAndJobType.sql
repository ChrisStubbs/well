CREATE PROCEDURE [dbo].[JobIds_GetByBranchAccountPickListRefAndJobType]
	@BranchId AS INT,
	@IdentifyJobTable dbo.IdentifyJobTableType	READONLY
AS
	SELECT 
		j.Id
	FROM 
		Job j
	INNER JOIN Stop s on s.id = j.StopId
	INNER JOIN RouteHeader rh on s.RouteHeaderId = rh.Id
	INNER JOIN @IdentifyJobTable idt 
			ON idt.PHAccount = j.PHAccount and idt.PicklistRef = j.PickListRef 
				and idt.JobTypeCode = j.JobTypeCode
	WHERE 
		rh.RouteOwnerId = @BranchId
	AND
		j.DateDeleted IS NOT NULL
		