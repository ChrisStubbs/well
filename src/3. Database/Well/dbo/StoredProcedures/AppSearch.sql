CREATE PROCEDURE [dbo].[AppSearch]
		@BranchId		INT				= NULL,
		@Date			DATE			= NULL,
		@Account		VARCHAR(20)		= NULL,
		@Invoice		VARCHAR(40)		= NULL,
		@Route			VARCHAR(12)		= NULL,
		@Driver			VARCHAR(50)		= NULL,
		@DeliveryType  	INT				= NULL,
		@Status			INT				= NULL
AS
BEGIN
	
	SELECT DISTINCT 
		rh.Id as RouteId
		,s.Id as StopId
		--,rh.RouteOwnerId as BranchId
		--,rh.RouteDate
		--,j.PHAccount
		--,a.Code
		--,a.Name
		--,j.InvoiceNumber
		--,rh.RouteNumber
		--,rh.DriverName
		----,jt.Id
		--,j.JobTypeCode
		----,js.Id
		--,js.Description
FROM	RouteHeader rh
INNER JOIN
		[Stop] s ON s.RouteHeaderId = rh.Id
INNER JOIN
		Job j ON j.StopId = s.Id
INNER JOIN
		Account a on a.StopId = s.Id
INNER JOIN 
		JobType jt on jt.Code =  j.JobTypeCode
INNER JOIN 
		JobStatus js on js.Id = j.JobStatusId
INNER JOIN 
		RouteStatusView rsv on rsv.RouteHeaderId = rh.Id
INNER JOIN 
		WellStatus ws on ws.Id = rsv.RouteStatus
INNER JOIN 
		StopStatusView ssv on ssv.StopId = s.Id
INNER JOIN 
		WellStatus ws2 on ws2.Id = ssv.WellStatusId
INNER JOIN 
		JobStatusView jsv on jsv.JobId = j.Id
INNER JOIN 
		WellStatus ws3 on ws3.Id = jsv.WellStatusId
WHERE 	(@BranchId IS NULL OR @BranchId = rh.RouteOwnerId)
		AND (@Date IS NULL OR @Date = rh.RouteDate)
		AND (@Account IS NULL OR (a.Code like '%'+ @Account+'%'  OR   a.Name like '%'+ @Account+'%'))
		AND (@Invoice IS NULL OR J.InvoiceNumber like '%'+ @Invoice+'%' )
		AND (@Route IS NULL OR rh.RouteNumber like '%'+ @Route +'%' )
		AND (@Driver IS NULL OR rh.DriverName like '%'+ @Driver +'%' )
		AND (@DeliveryType IS NULL OR @DeliveryType = jt.Id )
		AND (@Status IS NULL OR @Status = ws.Id OR @Status = ws2.Id OR @Status = ws3.Id)
		AND (j.JobTypeCode NOT IN ('DEL-DOC', 'NOTDEF', 'UPL-SAN'))
		   
END