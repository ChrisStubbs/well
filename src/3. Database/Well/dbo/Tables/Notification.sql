CREATE TABLE [dbo].[Notification]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[JobId] INT NOT NULL,
	[Type] TINYINT NOT NULL,
	[ErrorMessage] VARCHAR(255) NULL,
	[Branch] VARCHAR(3) NULL,
	[Account] VARCHAR(10) NULL,
	[InvoiceNumber] VARCHAR(20) NULL,
	[LineNumber] VARCHAR(3) NULL,
	[AdamErrorNumber] VARCHAR(3) NULL,
	[AdamCrossReference] VARCHAR(20) NULL,
	[UserName] VARCHAR(10) NULL,
	[IsArchived] BIT NOT NULL DEFAULT 0,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED ([Id] ASC)
)
