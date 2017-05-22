CREATE TABLE [dbo].[Audit]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Entry] varchar(8000)  NOT NULL,
	[Type] int  NOT NULL,
	[InvoiceNumber] varchar(255)  NOT NULL,
	[AccountCode] VARCHAR(255) NOT NULL,
	[DeliveryDate] DateTime NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_Audit] PRIMARY KEY CLUSTERED ([Id] ASC)
)
