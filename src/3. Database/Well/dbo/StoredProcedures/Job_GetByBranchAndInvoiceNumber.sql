CREATE PROCEDURE [dbo].[Job_GetByBranchAndInvoiceNumber]
	@jobId INT,
	@BranchId INT,
	@InvoiceNumber VARCHAR(40)
AS
BEGIN
	SELECT j.[Id]    
	FROM [dbo].[Job] j
	INNER JOIN Stop s ON j.StopId = s.Id 
	INNER JOIN RouteHeader r ON s.RouteHeaderId = r.Id
	where r.RouteOwnerId = @BranchId and
	j.InvoiceNumber = @InvoiceNumber and
	j.Id <> @jobId and
	j.JobTypeCode != 'DEL-DOC';

END

GO

