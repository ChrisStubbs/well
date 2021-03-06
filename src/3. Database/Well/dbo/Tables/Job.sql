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
	[OrdOuters] INT NULL DEFAULT 0,
	[InvOuters] INT NULL DEFAULT 0,
	[ColOuters] INT NULL DEFAULT 0,
	[ColBoxes] INT NULL DEFAULT 0,
	[ReCallPrd] BIT NULL DEFAULT 0,
	[AllowSOCrd] BIT NULL DEFAULT 0,
	[COD] VARCHAR(50) NULL,
	[GrnProcessType] INT NULL DEFAULT 0,
	[GrnNumber] VARCHAR(50) NULL,
	[GrnRefusedReason] VARCHAR(50) NULL,
	[GrnRefusedDesc] VARCHAR(50) NULL,
	[AllowReOrd] BIT NULL DEFAULT 0,
	[SandwchOrd] BIT NULL DEFAULT 0,
	[PerformanceStatusId] INT NULL,
	[Reason] VARCHAR(255) NULL,
    [DateDeleted] DATETIME NULL, 
	[DeletedByImport] BIT DEFAULT 0,
	[StopId] INT NOT NULL,
	[ActionLogNumber] VARCHAR(50) NULL,
	[OuterCount] INT NULL,
	[OuterDiscrepancyFound] BIT DEFAULT 0,
	[TotalOutersOver] INT NULL,
	[TotalOutersShort] INT NULL,
	[Picked] BIT NULL DEFAULT 0,
	[InvoiceValue] DECIMAL(8,2) NULL,
	[ProofOfDelivery] INT NULL DEFAULT 0,
	[DetailOutersOver] INT NULL,
	[DetailOutersShort] INT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	[JobStatusId] TINYINT NOT NULL DEFAULT 1,
	[ActivityId] INT NULL,
	ResolutionStatusId INT NULL CONSTRAINT FK_Job_ResolutionStatus FOREIGN KEY REFERENCES ResolutionStatus(Id), 
    [JobTypeId] TINYINT NOT NULL, 
	[WellStatusId] TINYINT NOT NULL, 
    CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Job_Stop] FOREIGN KEY ([StopId]) REFERENCES [dbo].[Stop] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_Job_JobStatus] FOREIGN KEY ([JobStatusId]) REFERENCES [dbo].[JobStatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_Job_ActivityId] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activity] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION, 
	CONSTRAINT [FK_Job_JobType] FOREIGN KEY([JobTypeId]) REFERENCES [dbo].[JobType] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_Job_WellStatus] FOREIGN KEY([WellStatusId]) REFERENCES [dbo].[WellStatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
GO
CREATE NONCLUSTERED INDEX [IDX_Job_JobStatusId] ON [dbo].[Job] ([JobStatusId]) INCLUDE ([JobTypeCode],[InvoiceNumber],[StopId])
GO
CREATE NONCLUSTERED INDEX [IDX_Job_StopId] ON [dbo].[Job] ([StopId]) INCLUDE ([Id],[JobTypeCode])
GO
GO CREATE NONCLUSTERED INDEX [IDX_Job_ActivityId] ON [dbo].[Job] ([ActivityId]) INCLUDE ([Id],[JobTypeCode],[StopId],[ResolutionStatusId])
GO
CREATE NONCLUSTERED INDEX [IDX_Job_DateDeleted] ON [dbo].[Job] ([DateDeleted]) INCLUDE ([JobTypeCode],[PHAccount],[PickListRef],[StopId])
GO
CREATE NONCLUSTERED INDEX IDX_Job_JobTypeCode_PHAccount_PickListRef_DateDeleted ON dbo.Job
(
	JobTypeCode ASC,
	PHAccount ASC,
	PickListRef ASC,
	DateDeleted ASC
)
INCLUDE (StopId) 
WHERE DateDeleted IS NULL
WITH (SORT_IN_TEMPDB = ON)
GO
CREATE NONCLUSTERED INDEX IDX_Job_DateDelete_ResolutionStatus ON Job
(
	DateDeleted ASC,
	ResolutionStatusId ASC
)
INCLUDE 
(
    PHAccount,
	InvoiceNumber,
	StopId
) 
WHERE DateDeleted IS NULL
WITH (SORT_IN_TEMPDB = ON)
GO