﻿CREATE TABLE [dbo].[RouteHeader]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[CompanyId] INT NOT NULL,
	[RouteNumber] NVARCHAR(12) NOT NULL,
	[RouteDate] DATETIME NOT NULL,
	[DriverName] NVARCHAR(50) NOT NULL,
	[VehicleReg] NVARCHAR(10) NOT NULL,
	[StartDepotCode] NVARCHAR(10) NOT NULL,
	[PlannedRouteStartTime] NVARCHAR(10) NOT NULL,
	[PlannedRouteFinishTime] NVARCHAR(10) NOT NULL,
	[PlannedDistance] DECIMAL(5,2) NOT NULL,
	[PlannedTravelTime] NVARCHAR(10) NOT NULL,
	[PlannedStops] TINYINT NOT NULL,
	[ActualStopsCompleted] TINYINT NULL,
	[RoutesId] INT NOT NULL,
	[RouteStatusId] TINYINT NULL,
	[RoutePerformanceStatusId] TINYINT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_RouteHeader] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_RouteHeader_Routes] FOREIGN KEY ([RoutesId]) REFERENCES [dbo].[Routes] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_RouteHeader_RouteStatusId] FOREIGN KEY ([RouteStatusId]) REFERENCES [dbo].[Routestatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_RouteHeader_RoutePerformanceStatusId] FOREIGN KEY ([RoutePerformanceStatusId]) REFERENCES [dbo].[RoutePerformanceStatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
