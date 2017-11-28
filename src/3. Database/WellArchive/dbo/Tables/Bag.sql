CREATE TABLE [dbo].[Bag]
(
	[Id] INT NOT NULL,
	DataSource VarChar(255) NULL,
	[Barcode] VARCHAR(50) NOT NULL,
	[Description] VARCHAR(255) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
    [DateDeleted] DATETIME NULL,
	[ArchiveDate] SMALLDATETIME NOT NULL
)
