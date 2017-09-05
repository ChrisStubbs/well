CREATE PROCEDURE DeleteRouteHeaderWithNoStops
	@UpdatedBy varchar(50)
AS
	UPDATE RouteHeader 
	SET 
		DateDeleted = GETDATE(),
		UpdatedBy = @UpdatedBy
	WHERE 
		Id IN 
        (
        	SELECT rh.Id 
	        FROM 
                RouteHeader rh
	            LEFT JOIN Stop s on s.RouteHeaderId = rh.Id
	        WHERE
		        rh.DateDeleted IS NULL
		        AND s.DateDeleted IS NULL
		        AND s.Id IS NULL
        )