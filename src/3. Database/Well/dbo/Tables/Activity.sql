CREATE TABLE [dbo].[Activity]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[DocumentNumber] VARCHAR(40) NULL,
	[InitialDocument] VARCHAR(40) NOT NULL,
	[ActivityTypeId] TINYINT NOT NULL,
	[LocationId] INT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    [DateDeleted] DATETIME NULL, 
    CONSTRAINT [PK_Activity] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Activity_ActivityType] FOREIGN KEY ([ActivityTypeId]) REFERENCES [dbo].[ActivityType] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_Activity_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
GO
CREATE NONCLUSTERED INDEX [IDX_Activity_DocumentNumber]	ON [dbo].[Activity] ([DocumentNumber])
GO
CREATE NONCLUSTERED INDEX [Idx_Activity_InitialDocument_ActivityTypeId_LocationId] ON [dbo].[Activity] ([InitialDocument],[ActivityTypeId],[LocationId]) 
INCLUDE ([Id])
GO
