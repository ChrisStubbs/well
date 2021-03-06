﻿CREATE TABLE [dbo].[LineItemAction]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[ExceptionTypeId] INT NOT NULL,
	[Quantity] INT NOT NULL,
	[SourceId] TINYINT NULL,
	[ReasonId] TINYINT NULL,
	[ReplanDate] DATETIME NULL,
	[SubmittedDate] DATETIME NULL,
	[ApprovalDate] DATETIME NULL,
	[ApprovedBy] VARCHAR(50) NULL,
	[LineItemId] INT NOT NULL,
	[Originator] TINYINT NULL,
	[ActionedBy] VARCHAR(50) NULL,
	[DeliveryActionId] INT NULL,
	[PDAReasonDescription] VARCHAR(50) NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
    [DateDeleted] DATETIME NULL, 
	[Version] [TIMESTAMP] NOT NULL,
	[DeletedByImport] BIT DEFAULT 0,
    CONSTRAINT [PK_LineItemAction] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_LineItemAction_LineItem] FOREIGN KEY ([LineItemId]) REFERENCES [dbo].[LineItem] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_LineItemAction_ExceptionType] FOREIGN KEY ([ExceptionTypeId]) REFERENCES [dbo].[ExceptionType] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_LineItemAction_DeliveryAction] FOREIGN KEY ([DeliveryActionId]) REFERENCES [dbo].[DeliveryAction] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
GO

CREATE NONCLUSTERED INDEX [Idx_LineItemAction_DateDeleted] ON [dbo].[LineItemAction] (DateDeleted ASC)
INCLUDE (LineItemId) 
WHERE datedeleted is null
WITH (SORT_IN_TEMPDB = ON)
GO

CREATE NONCLUSTERED INDEX [Idx_LineItemAction_LineItemId] ON [dbo].[LineItemAction] ([LineItemId])
INCLUDE ([DateDeleted])
WHERE datedeleted is null
GO
