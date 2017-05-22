CREATE TABLE [dbo].[AccountType]
(
	[Id] [TINYINT] IDENTITY(1,1) NOT NULL,
	[Code] VARCHAR(20) NOT NULL,
    [Description] VARCHAR(20) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_AccountType] PRIMARY KEY CLUSTERED ([Id] ASC),
)
