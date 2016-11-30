﻿CREATE PROCEDURE [dbo].[RouteHeader_Insert]
	@CompanyId			    INT,
	@RouteNumber			VARCHAR(50),
	@RouteOwnerId			INT,
	@RouteDate				DATETIME,
	@DriverName				VARCHAR(50) = NULL,
	@StartDepotCode			INT,
	@PlannedStops			TINYINT,
	@ActualStopsCompleted   TINYINT = 0, 
	@RoutesId				INT,
	@RouteStatusId			TINYINT= NULL,
	@RoutePerformanceStatusId TINYINT= NULL,
	@LastRouteUpdate		DATETIME = GETDATE,
	@AuthByPass			    INT = NULL,
	@NonAuthByPass		    INT = NULL,
	@ShortDeliveries        INT = NULL,
	@DamagesRejected        INT = NULL,
	@DamagesAccepted        INT = NULL,
	@CreatedBy				VARCHAR(50),
	@CreatedDate DATETIME,
	@UpdatedBy VARCHAR(50),
	@UpdatedDate DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	INSERT RouteHeader (
		[CompanyId],
		[RouteNumber],
		[RouteOwnerId],
		[RouteDate],
		[DriverName],
		[StartDepotCode],
		[PlannedStops], 
		[ActualStopsCompleted],  
		[RoutesId], 
		[RouteStatusId], 
		[RoutePerformanceStatusId], 
		[LastRouteUpdate],
		[AuthByPass], 
		[NonAuthByPass], 
		[ShortDeliveries], 
		[DamagesRejected], 
		[DamagesAccepted], 
		[CreatedBy],
		[DateCreated],
		[UpdatedBy],
		[DateUpdated])
	VALUES (
		@CompanyId, 
		@RouteNumber, 
		@RouteOwnerId,
		@RouteDate, 
		@DriverName,  
		@StartDepotCode,
		@PlannedStops,
		@ActualStopsCompleted, 
		@RoutesId, 
		@RouteStatusId, 
		@RoutePerformanceStatusId, 
		@LastRouteUpdate, 
		@AuthByPass, 
		@NonAuthByPass, 
		@ShortDeliveries, 
		@DamagesRejected,
		@DamagesAccepted, 
		@CreatedBy, 
		@CreatedDate, 
		@UpdatedBy, 
		@UpdatedDate)

		SELECT CAST(SCOPE_IDENTITY() as int);

END
