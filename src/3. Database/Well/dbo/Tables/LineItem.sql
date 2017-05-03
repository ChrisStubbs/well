CREATE TABLE [dbo].[LineItem]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[ProductCode] VARCHAR(60) NOT NULL,
	[ProductDescription] VARCHAR(100) NOT NULL,
	[AmendedDeliveryQuantity] INT NULL,
	[AmendedShortQuantity] INT NULL,
	[OriginalShortQuantity] INT NOT NULL,
	[OptionalBagBarcode] VARCHAR (50) NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_LineItem] PRIMARY KEY CLUSTERED ([Id] ASC)
)
