CREATE TABLE [dbo].[LineItemAction]
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
	[Originator] VARCHAR(50) NULL,
	[ActionedBy] VARCHAR(50) NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_LineItemAction] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_LineItemAction_LineItem] FOREIGN KEY ([LineItemId]) REFERENCES [dbo].[LineItem] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_LineItemAction_ExceptionType] FOREIGN KEY ([ExceptionTypeId]) REFERENCES [dbo].[ExceptionType] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
