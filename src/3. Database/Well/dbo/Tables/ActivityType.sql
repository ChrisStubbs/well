CREATE TABLE [dbo].[ActivityType]
(
	[Id] [TINYINT] IDENTITY(1,1) NOT NULL,
	[DisplayName] VARCHAR(20) NULL,
	[Description] VARCHAR(50) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_ActivityType] PRIMARY KEY CLUSTERED ([Id] ASC),
)
