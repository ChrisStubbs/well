CREATE TABLE [dbo].[ThresholdLevel]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Description] VARCHAR(255) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_ThresholdLevel] PRIMARY KEY CLUSTERED ([Id] ASC)
)
