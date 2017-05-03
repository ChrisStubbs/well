CREATE TABLE [dbo].[LineItemAction]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[ExceptionType] TINYINT NOT NULL,
	[Quantity] INT NOT NULL,
	[SourceId] TINYINT NULL,
	[ReasonId] TINYINT NULL,
	[ReplanDate] DATETIME NULL,
	[SubmittedDate] DATETIME NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_LineItemAction] PRIMARY KEY CLUSTERED ([Id] ASC)
)
