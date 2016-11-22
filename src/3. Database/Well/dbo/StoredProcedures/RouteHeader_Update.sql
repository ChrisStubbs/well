CREATE PROCEDURE [dbo].[RouteHeader_Update]
	@Id INT,
	@RouteStatusId		    INT,
	@RoutePerformanceStatusId INT,
	@LastRouteUpdate			DATETIME,
	@AuthByPass				INT,
	@NonAuthByPass			INT,
	@ShortDeliveries		INT,
	@DamagesRejected		INT, 
	@DamagesAccepted		INT,
	@NotRequired			INT,
	@UpdatedBy VARCHAR(50),
	@UpdatedDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE RouteHeader SET 
		RouteStatusId = @RouteStatusId,
		RoutePerformanceStatusId = @RoutePerformanceStatusId,
		LastRouteUpdate = @LastRouteUpdate,
		AuthByPass = @AuthByPass,
		ShortDeliveries = @ShortDeliveries,
		DamagesRejected = @DamagesRejected,
		DamagesAccepted = @DamagesAccepted,
		NotRequired = @NotRequired,
		UpdatedBy = @UpdatedBy,
		DateUpdated = @UpdatedDate
	WHERE Id = @Id

END
