CREATE PROCEDURE [dbo].[RouteHeader_Update]
	@Id INT,
	@RouteStatusCode		    VARCHAR(50) = NULL,
	@RouteStatusDescription VARCHAR(255) = NULL,
	@PerformanceStatusCode VARCHAR(50) = NULL,
	@PerformanceStatusDescription VARCHAR(255) = NULL,
	@LastRouteUpdate			DATETIME,
	@AuthByPass				INT,
	@NonAuthByPass			INT,
	@ShortDeliveries		INT,
	@DamagesRejected		INT, 
	@DamagesAccepted		INT,
	@DriverName VARCHAR(255),
	@UpdatedBy VARCHAR(50),
	@UpdatedDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE RouteHeader SET 
		RouteStatusCode = @RouteStatusCode,
		RouteStatusDescription = @RouteStatusDescription,
		PerformanceStatusCode = @PerformanceStatusCode,
		PerformanceStatusDescription = @PerformanceStatusDescription,
		LastRouteUpdate = @LastRouteUpdate,
		AuthByPass = @AuthByPass,
		ShortDeliveries = @ShortDeliveries,
		DamagesRejected = @DamagesRejected,
		DamagesAccepted = @DamagesAccepted,
		DriverName = @DriverName,
		UpdatedBy = @UpdatedBy,
		DateUpdated = @UpdatedDate
	WHERE Id = @Id

END
