CREATE TABLE [dbo].[LineItem]
(
	[Id] INT NOT NULL,
	DataSource VarChar(255) NULL,
	[LineNumber] SMALLINT NULL,
	[ProductCode] VARCHAR(60) NOT NULL,
	[ProductDescription] VARCHAR(100) NOT NULL,
	[AmendedDeliveryQuantity] INT NULL,
	[AmendedShortQuantity] INT NULL,
	[OriginalShortQuantity] INT NULL,
	[BagId] INT NULL,
	[ActivityId] INT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] SMALLDATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] SMALLDATETIME NOT NULL,
    [DateDeleted] SMALLDATETIME NULL, 
	[DeletedByImport] BIT DEFAULT 0,
	[JobId] INT NULL,
    [ArchiveDate] SMALLDATETIME NOT NULL
)