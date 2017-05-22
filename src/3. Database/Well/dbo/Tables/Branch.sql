CREATE TABLE [dbo].[Branch]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
	[TranscendMapping] VARCHAR(10) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_Branch] PRIMARY KEY CLUSTERED ([Id] ASC)
)
