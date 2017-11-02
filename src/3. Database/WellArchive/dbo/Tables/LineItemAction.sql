CREATE TABLE [dbo].[LineItemAction]
(
	[Id] INT NOT NULL,
	[ExceptionTypeId] INT NOT NULL,
	[Quantity] INT NOT NULL,
	[SourceId] TINYINT NULL,
	[ReasonId] TINYINT NULL,
	[ReplanDate] DATETIME NULL,
	[SubmittedDate] DATETIME NULL,
	[ApprovalDate] DATETIME NULL,
	[ApprovedBy] VARCHAR(50) NULL,
	[LineItemId] INT NOT NULL,
	[Originator] TINYINT NULL,
	[ActionedBy] VARCHAR(50) NULL,
	[DeliveryActionId] INT NULL,
	[PDAReasonDescription] VARCHAR(50) NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
    [DateDeleted] DATETIME NULL, 
	[DeletedByImport] BIT DEFAULT 0,
	[ArchiveDate] SMALLDATETIME NOT NULL
)

