CREATE TABLE [archive].[Bag]
(
	[Id] INT NOT NULL,
	[Barcode] VARCHAR(50) NOT NULL,
	[Description] VARCHAR(255) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
    [DateDeleted] DATETIME NULL,
	[DateArchived] SMALLDATETIME NOT NULL
)ON Archive
