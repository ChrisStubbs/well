CREATE TABLE [dbo].[JobDetailDamage]
(
	[Id] INT NOT NULL,
	DataSource VarChar(255) NULL,
	[JobDetailId] INT NOT NULL,
	[Qty] INT NULL,
	[DateDeleted] DATETIME NULL, 
	[DeletedByImport] BIT DEFAULT 0,
	JobDetailSourceId TINYINT,
	JobDetailReasonId TINYINT,
	DamageActionId INT NULL,
	DamageStatus INT NULL,
	PdaReasonDescription VARCHAR(50) NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
    [ArchiveDate] SMALLDATETIME NOT NULL
)
