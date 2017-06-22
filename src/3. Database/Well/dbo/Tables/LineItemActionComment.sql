CREATE TABLE [dbo].[LineItemActionComment]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[LineItemActionId] INT NOT NULL,
	[CommentReasonId] INT NOT NULL,
	[FromQty] INT NULL DEFAULT 0,
	[ToQty] INT NOT NULL DEFAULT 0,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[DateDeleted] DATETIME NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_LineItemActionComment] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_LineItemActionComment_LineItemAction] FOREIGN KEY ([LineItemActionId]) REFERENCES [dbo].[LineItemAction] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_LineItemActionComment_CommentReason] FOREIGN KEY ([CommentReasonId]) REFERENCES [dbo].[CommentReason] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
