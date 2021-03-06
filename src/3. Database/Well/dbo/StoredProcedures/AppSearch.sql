﻿CREATE PROCEDURE [dbo].[AppSearch]
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
		,rh.RouteOwnerId as BranchId
		,j.InvoiceNumber as InvoiceNumber
FROM	RouteHeader rh
INNER JOIN
		[Stop] s ON s.RouteHeaderId = rh.Id
INNER JOIN
		Job j ON j.StopId = s.Id
INNER JOIN
		Account a on a.StopId = s.Id
INNER JOIN 
		JobType jt on jt.Id =  j.JobTypeId
INNER JOIN 
		JobStatus js on js.Id = j.JobStatusId
INNER JOIN 
		RouteStatusView rsv on rsv.RouteHeaderId = rh.Id
INNER JOIN 
		WellStatus ws on ws.Id = rsv.RouteStatus
INNER JOIN 
		WellStatus ws2 on ws2.Id = s.WellStatus
INNER JOIN 
		WellStatus ws3 on ws3.Id = j.WellStatusId
WHERE 	(@BranchId IS NULL OR @BranchId = rh.RouteOwnerId)
		AND (@Date IS NULL OR @Date = rh.RouteDate)
		AND (@Account IS NULL OR (a.Code like '%'+ @Account+'%'  OR   a.Name like '%'+ @Account+'%'))
		AND (@Invoice IS NULL OR J.InvoiceNumber like '%'+ @Invoice+'%' )
		AND (@Route IS NULL OR rh.RouteNumber like '%'+ @Route +'%' )
		AND (@Driver IS NULL OR rh.DriverName like '%'+ @Driver +'%' )
		AND (@DeliveryType IS NULL OR @DeliveryType = jt.Id )
		AND (@Status IS NULL OR @Status = ws.Id OR @Status = ws2.Id OR @Status = ws3.Id)
		--AND (j.JobTypeCode NOT IN ('DEL-DOC', 'NOTDEF', 'UPL-SAN'))
		   
END