CREATE TABLE [dbo].[RoyaltyExceptions]
(
	[Id] [TINYINT] IDENTITY(1,1) NOT NULL,
	[RoyaltyCode] INT NOT NULL,
	[ExceptionHours] TINYINT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_RoyaltyExceptions] PRIMARY KEY CLUSTERED ([Id] ASC),
)
