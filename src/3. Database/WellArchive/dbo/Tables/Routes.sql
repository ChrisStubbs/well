CREATE TABLE [dbo].[Routes]
(
	[Id] INT NOT NULL,
	[FileName] VARCHAR(255),
	[DateDeleted] DATETIME NULL, 
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[ArchiveDate] SMALLDATETIME NOT NULL
)