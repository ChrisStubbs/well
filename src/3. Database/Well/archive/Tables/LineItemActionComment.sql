CREATE TABLE [archive].[LineItemActionComment]
(
	[Id] INT NOT NULL,
	[LineItemActionId] INT NOT NULL,
	[CommentReasonId] INT NOT NULL,
	[FromQty] INT NULL DEFAULT 0,
	[ToQty] INT NOT NULL DEFAULT 0,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[DateDeleted] DATETIME NULL,
	[DeletedByImport] BIT DEFAULT 0,
	[Version] [TIMESTAMP] NOT NULL,
	[ArchiveDate] SMALLDATETIME NOT NULL
)ON Archive
