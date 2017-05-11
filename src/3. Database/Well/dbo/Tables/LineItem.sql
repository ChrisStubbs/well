CREATE TABLE [dbo].[LineItem]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[LineNumber] INT NULL,
	[ProductCode] VARCHAR(60) NOT NULL,
	[ProductDescription] VARCHAR(100) NOT NULL,
	[AmendedDeliveryQuantity] INT NULL,
	[AmendedShortQuantity] INT NULL,
	[OriginalShortQuantity] INT NULL,
	[BagId] INT NULL,
	[ActivityId] INT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_LineItem] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_LineItem_Bag] FOREIGN KEY ([BagId]) REFERENCES [dbo].[Bag] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION,
	CONSTRAINT [FK_LineItem_Activity] FOREIGN KEY ([ActivityId]) REFERENCES [dbo].[Activity] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
