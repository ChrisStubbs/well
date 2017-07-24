CREATE TABLE [dbo].[ExceptionEvent]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Event] VARCHAR(MAX) NOT NULL,
	[ExceptionActionId] INT NOT NULL,
	[Processed] BIT NOT NULL DEFAULT 0,
	[DateCanBeProcessed] DATETIME NOT NULL,
	[EntityId] VARCHAR(50) NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_ExceptionEvent] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_ExceptionAction] FOREIGN KEY ([ExceptionActionId]) REFERENCES [dbo].[ExceptionAction] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
