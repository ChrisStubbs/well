CREATE PROCEDURE UpdateRouteHeaderFieldsFromImportedFile
	@Id				INT,
	@PlannedStops	TINYINT,
	@RouteDate		SmallDateTime,
	@RouteNumber	VarChar(12), 
	@RouteOwnerId	INT,
	@StartDepot		INT
AS	
	UPDATE RouteHeader SET 
		PlannedStops = @PlannedStops,
		RouteDate = @RouteDate,
		RouteNumber = @RouteNumber,
		RouteOwnerId = @RouteOwnerId,
		StartDepotCode = @StartDepot
	WHERE Id = @Id