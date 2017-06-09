CREATE TABLE [dbo].[LineItemActionComment]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[LineItemAction_Id] INT NOT NULL,
	[CommentReason_Id] INT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_LineItemActionComment] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_LineItemActionComment_LineItemAction] FOREIGN KEY ([LineItemAction_Id]) REFERENCES [dbo].[LineItemAction] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_LineItemActionComment_CommentReason] FOREIGN KEY ([CommentReason_Id]) REFERENCES [dbo].[CommentReason] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
