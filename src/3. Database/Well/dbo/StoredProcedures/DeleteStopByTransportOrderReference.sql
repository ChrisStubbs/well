CREATE PROCEDURE  [dbo].[DeleteStopByTransportOrderReference]
	@TransportOrderReference VARCHAR(50)
AS
BEGIN

	DELETE d FROM JobDetailDamage d 
	JOIN JobDetail jd on jd.Id = d.JobDetailId
	JOIN Job j on j.Id = jd.JobId
	JOIN [Stop] s on s.Id = j.StopId
	WHERE s.TransportOrderReference = @TransportOrderReference

	DELETE jd FROM JobDetail jd
	JOIN Job j on j.Id = jd.JobId
	JOIN [Stop] s on s.Id = j.StopId
	WHERE s.TransportOrderReference = @TransportOrderReference

	DELETE j FROM Job j
	JOIN [Stop] s on s.Id = j.StopId
	WHERE s.TransportOrderReference = @TransportOrderReference

	DELETE s FROM [Stop] s 
	WHERE s.TransportOrderReference = @TransportOrderReference

END