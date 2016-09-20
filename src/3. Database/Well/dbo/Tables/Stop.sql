﻿CREATE TABLE [dbo].[Stop]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[PlannedStopNumber] VARCHAR(4) NOT NULL,
	[RouteHeaderCode] VARCHAR(10)  NOT NULL,
	[RouteHeaderId] INT NOT NULL,
	[DropId] VARCHAR(2) NOT NULL,
	[LocationId] VARCHAR(20) NOT NULL,
	[DeliveryDate] DATETIME NOT NULL,
	[ShellActionIndicator] varchar(100) NULL,
	[CustomerShopReference] varchar(100) NULL,
	[AllowOvers] BIT NULL,
	[CustUnatt] BIT NULL,
	[PHUnatt] BIT NULL,
	[StopStatusId] TINYINT NULL,
	[StopPerformanceStatusId] TINYINT NULL,
	[ByPassReasonId] TINYINT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_Stops] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Stops_RouteHeader] FOREIGN KEY ([RouteHeaderId]) REFERENCES [dbo].[RouteHeader] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_Stops_StopStatus] FOREIGN KEY ([StopStatusId]) REFERENCES [dbo].[StopStatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_Stops_StopPerformanceStatus] FOREIGN KEY ([StopPerformanceStatusId]) REFERENCES [dbo].[PerformanceStatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
)
