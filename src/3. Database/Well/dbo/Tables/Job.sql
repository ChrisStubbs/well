﻿CREATE TABLE [dbo].[Job]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Sequence] INT NOT NULL,
	[JobTypeCode] VARCHAR(10) NOT NULL,
	[PHAccount] VARCHAR(40) NOT NULL,
	[PickListRef] VARCHAR(40) NULL,
	[InvoiceNumber] VARCHAR(40) NULL,
	[CustomerRef] VARCHAR(40) NULL,
	[OrderDate] DATETIME NULL,
	[RoyaltyCode] VARCHAR(60) NULL,
	[RoyaltyCodeDesc] VARCHAR(50) NULL,
	[OrdOuters] BIT NULL DEFAULT 0,
	[InvOuters] BIT NULL DEFAULT 0,
	[ColOuters] BIT NULL DEFAULT 0,
	[ColBoxes] BIT NULL DEFAULT 0,
	[ReCallPrd] BIT NULL DEFAULT 0,
	[AllowSgCrd] BIT NULL DEFAULT 0,
	[AllowSOCrd] BIT NULL DEFAULT 0,
	[COD] BIT NULL DEFAULT 0,
	[GrnNumber] VARCHAR(50) NULL,
	[GrnRefusedReason] VARCHAR(50) NULL,
	[GrnRefusedDesc] VARCHAR(50) NULL,
	[AllowReOrd] BIT NULL DEFAULT 0,
	[SandwchOrd] BIT NULL DEFAULT 0,
	[ComdtyType] NVARCHAR(1) NULL,
	[TotalCreditValueForThreshold] DECIMAL(8,2) NULL,
	[PerformanceStatusId] TINYINT NULL,
	[ByPassReasonId] TINYINT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0,
	[StopId] INT NOT NULL,
	[ActionLogNumber] VARCHAR(50) NULL,
	[OuterCount] TINYINT NULL,
	[OuterDiscrepancyFound] VARCHAR(1) NULL,
	[TotalOutersOver] INT NULL,
	[TotalOutersShort] INT NULL,
	[Picked] BIT NULL DEFAULT 0,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Job_Stop] FOREIGN KEY ([StopId]) REFERENCES [dbo].[Stop] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
