﻿CREATE TABLE [dbo].[RouteHeader]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[CompanyId] INT NOT NULL,
	[RouteNumber] NVARCHAR(12) NOT NULL,
	[RouteDate] DATETIME NOT NULL,
	[DriverName] NVARCHAR(50) NOT NULL,
	[VehicleReg] NVARCHAR(10) NOT NULL,
	[StartDepotCode] NVARCHAR(10) NOT NULL,
	[StartDepotId] NVARCHAR(5) NOT NULL,
	[FinishDepotCode] NVARCHAR(10) NOT NULL,
	[FinishDepotId] NVARCHAR(5) NOT NULL,
	[SubDepotCode] NVARCHAR(10) NOT NULL,
	[SubDepotId] NVARCHAR(5) NOT NULL,
	[FinishSubDepotCode] NVARCHAR(10) NOT NULL,
	[FinishSubDepotId] NVARCHAR(5) NOT NULL,
	[PlannedRouteStartTime] TIME NOT NULL,
	[PlannedRouteFinishTime] TIME NOT NULL,
	[InitialSealNumber] NVARCHAR(20) NULL,
	[PlannedDistance] DECIMAL(3,2) NOT NULL,
	[PlannedTravelTime] TIME NOT NULL,
	[PlannedStops] TINYINT NOT NULL,
	[ActualStopsCompleted] TINYINT NOT NULL,
	[AuthByPass] INT NULL,
	[NonAuthByPass] INT NOT NULL,
	[ShortDeliveries] INT NOT NULL,
	[DamagesRejected] INT NOT NULL,
	[DamagesAccepted] INT NOT NULL,
	[NotRequired] INT NOT NULL, 
	[RouteImportId] INT NOT NULL,
	[RouteStatusId] TINYINT NULL,
	[RoutePerformanceStatusId] TINYINT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_RouteHeader] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_RouteHeader_RouteImport] FOREIGN KEY ([RouteImportId]) REFERENCES [dbo].[RouteImport] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_RouteHeader_RouteStatusId] FOREIGN KEY ([RouteStatusId]) REFERENCES [dbo].[Routestatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_RouteHeader_RoutePerformanceStatusId] FOREIGN KEY ([RoutePerformanceStatusId]) REFERENCES [dbo].[RoutePerformanceStatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)