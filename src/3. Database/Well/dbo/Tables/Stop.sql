﻿CREATE TABLE [dbo].[Stop]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[PlannedStopNumber] VARCHAR(4) NOT NULL,
	[PlannedArriveTime] VARCHAR(12) NOT NULL,
	[PlannedDepartTime] VARCHAR(12) NOT NULL,
	[RouteHeaderCode] VARCHAR(10)  NOT NULL,
	[RouteHeaderId] INT NOT NULL,
	[DropId] VARCHAR(2) NOT NULL,
	[LocationId] VARCHAR(20) NOT NULL,
	[DeliveryDate] DATETIME NOT NULL,
	[SpecialInstructions] VARCHAR(100) NOT NULL,
	[StartWindow] VARCHAR(12) NOT NULL,
	[EndWindow] VARCHAR(12) NOT NULL,
	[TextField1] varchar(100) NULL,
	[TextField2] varchar(100) NULL,
	[TextField3] varchar(100) NULL,
	[TextField4] varchar(100) NULL,
	[StopStatusId] TINYINT NULL,
	[StopPerformanceStatusId] TINYINT NULL,
	[ByPassReasonId] TINYINT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	[IsDeleted] BIT NOT NULL, 
    CONSTRAINT [PK_Stops] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Stops_RouteHeader] FOREIGN KEY ([RouteHeaderId]) REFERENCES [dbo].[RouteHeader] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_Stops_StopStatus] FOREIGN KEY ([StopStatusId]) REFERENCES [dbo].[StopStatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_Stops_StopPerformanceStatus] FOREIGN KEY ([StopPerformanceStatusId]) REFERENCES [dbo].[PerformanceStatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
)
