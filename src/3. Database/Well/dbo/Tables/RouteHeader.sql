﻿CREATE TABLE [dbo].[RouteHeader]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[CompanyId] INT NOT NULL,
	[RouteNumber] VARCHAR(12) NOT NULL,
	[RouteDate] DATETIME NULL,
	[DriverName] VARCHAR(50) NULL,
	[StartDepotCode] INT NOT NULL,
	[PlannedStops] TINYINT NOT NULL,
	[ActualStopsCompleted] TINYINT NULL,
	[RoutesId] INT NOT NULL,
	[RouteStatusCode] VARCHAR(50) NULL,
	[RouteStatusDescription] VARCHAR(255) NULL,
	[PerformanceStatusCode] VARCHAR(50) NULL,
	[PerformanceStatusDescription] VARCHAR(255) NULL,
	[LastRouteUpdate] DATETIME NULL,
	[AuthByPass] INT NULL,
	[NonAuthByPass] INT NULL,
	[ShortDeliveries] INT NULL,
	[DamagesRejected] INT NULL,
	[DamagesAccepted] INT NULL,
	[RouteOwnerId] INT NOT NULL,
	[WellStatus] TINYINT NULL,
	[DateDeleted] DATETIME NULL, 
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	[HasNotDefinedDeliveryAction] BIT NULL,
	[NoGRNButNeeds] BIT NULL,
	[PendingSubmission] BIT NULL,
	[ExceptionCount] TINYINT NULL,
	[CleanCount] TINYINT NULL,
    CONSTRAINT [PK_RouteHeader] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_RouteHeader_Routes] FOREIGN KEY ([RoutesId]) REFERENCES [dbo].[Routes] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_RouteHeader_StartDepotCode] FOREIGN KEY ([StartDepotCode]) REFERENCES [dbo].[Branch] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_RouteHeader_RouteOwnerId] FOREIGN KEY ([RouteOwnerId]) REFERENCES [dbo].[Branch] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_RouteHeader_WellStatus] FOREIGN KEY ([WellStatus]) REFERENCES [dbo].[WellStatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
