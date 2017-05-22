CREATE TABLE [dbo].[ExceptionAction]
(
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[Description] VARCHAR(50) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_ExceptionAction] PRIMARY KEY CLUSTERED ([Id] ASC),
)
