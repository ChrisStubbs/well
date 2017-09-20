CREATE PROCEDURE UpdateRouteHeaderFieldsFromImportedFile
	@Id				INT,
	@PlannedStops	TINYINT,
	@RouteDate		SmallDateTime,
	@RouteNumber	VarChar(12), 
	@RouteOwnerId	INT,
	@StartDepotCode	INT
AS	
	UPDATE RouteHeader SET 
		PlannedStops = @PlannedStops,
		RouteDate = @RouteDate,
		RouteNumber = @RouteNumber,
		RouteOwnerId = @RouteOwnerId,
		StartDepotCode = @StartDepotCode
	WHERE Id = @Id