CREATE PROCEDURE  [dbo].[DeleteStopByTransportOrderReference]
	@TransportOrderReference VARCHAR(50),
	@UpdatedBy  VARCHAR(50)
AS
BEGIN
	--LineItemAction
	UPDATE lia  
	SET lia.DateDeleted = GetDate(),
		lia.LastUpdatedBy = @UpdatedBy
	FROM LineItemAction lia
	INNER JOIN LineItem li on lia.LineItemId = li.Id
	INNER JOIN JobDetail jd on jd.LineItemId = li.Id
	INNER JOIN Job j on jd.JobId = j.Id
	INNER JOIN Stop s on j.StopId = s.Id
	WHERE 
		s.TransportOrderReference = @TransportOrderReference
		AND lia.DateDeleted IS NULL
		
	--LineItem
	UPDATE li 
	SET li.DateDeleted = GetDate(),
		li.LastUpdatedBy = @UpdatedBy
	FROM LineItem li
	INNER JOIN JobDetail jd on jd.LineItemId = li.Id
	INNER JOIN Job j on jd.JobId = j.Id
	INNER JOIN Stop s on j.StopId = s.Id
	WHERE 
		s.TransportOrderReference = @TransportOrderReference
		AND li.DateDeleted IS NULL
	
	--job detail damage
	UPDATE jdd 
	SET jdd.DateDeleted = GetDate(),
		jdd.UpdatedBy = @UpdatedBy
	FROM JobDetailDamage jdd
	INNER JOIN JobDetail jd on jdd.JobDetailId = jd.Id
	INNER JOIN Job j on jd.JobId = j.Id
	INNER JOIN Stop s on j.StopId = s.Id
	WHERE 
		s.TransportOrderReference = @TransportOrderReference
		AND jdd.DateDeleted IS NULL

	--JobDetail
	UPDATE jd 
	SET jd.DateDeleted = GetDate(),
		jd.UpdatedBy = @UpdatedBy
	FROM JobDetail jd
	INNER JOIN Job j on jd.JobId = j.Id
	INNER JOIN Stop s on j.StopId = s.Id
	WHERE 
		s.TransportOrderReference = @TransportOrderReference
		AND Jd.DateDeleted IS NULL

	-- job
	UPDATE j 
	SET j.DateDeleted = GetDate(),
		j.UpdatedBy = @UpdatedBy
	FROM  Job j
	INNER JOIN Stop s on j.StopId = s.Id
	WHERE 
		s.TransportOrderReference = @TransportOrderReference
		AND j.DateDeleted IS NULL
	
	-- stop
	UPDATE s 
	SET s.DateDeleted = GetDate(),
		s.UpdatedBy = @UpdatedBy
	FROM  Stop s
	WHERE 
		s.TransportOrderReference = @TransportOrderReference
		AND s.DateDeleted IS NULL
	
	-- Account
	UPDATE a 
	SET a.DateDeleted = GetDate(),
		a.UpdatedBy = @UpdatedBy
	FROM  Account a
	INNER JOIN Stop s on a.StopId = s.Id
	WHERE 
		s.TransportOrderReference = @TransportOrderReference
		AND a.DateDeleted IS NULL
END