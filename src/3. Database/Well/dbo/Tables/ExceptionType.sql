CREATE TABLE [dbo].[ExceptionType]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Description] VARCHAR(20) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_ExceptionType] PRIMARY KEY CLUSTERED ([Id] ASC),
)
