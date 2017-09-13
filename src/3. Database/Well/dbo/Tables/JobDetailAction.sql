CREATE TABLE [dbo].[JobDetailAction]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[JobDetailId] INT NOT NULL,	
	[Quantity] INT NOT NULL,
	[ActionId] INT NOT NULL,
	[StatusId] INT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_JobDetailAction] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_JobDetailAction_JobDetail] FOREIGN KEY ([JobDetailId]) REFERENCES [dbo].[JobDetail] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_JobDetailAction_ExceptionAction] FOREIGN KEY ([ActionId]) REFERENCES [dbo].[ExceptionAction] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_JobDetailAction_ActionStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[ActionStatus] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
)
