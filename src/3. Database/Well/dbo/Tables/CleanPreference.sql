CREATE TABLE [dbo].[CleanPreference]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Days] INT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
    [DateDeleted] DATETIME NULL, 
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_CleanPreference] PRIMARY KEY CLUSTERED ([Id] ASC)
)
