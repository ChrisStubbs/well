CREATE TABLE [dbo].[PDACreditReasons]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[CreditReason] VARCHAR(30) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_PDACreditReasons] PRIMARY KEY CLUSTERED ([Id] ASC),
)
