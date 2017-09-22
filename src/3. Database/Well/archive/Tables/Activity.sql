CREATE TABLE [archive].[Activity]
(
	[Id] INT NOT NULL,
	[DocumentNumber] VARCHAR(40) NULL,
	[InitialDocument] VARCHAR(40) NOT NULL,
	[ActivityTypeId] TINYINT NOT NULL,
	[LocationId] INT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    [DateDeleted] DATETIME NULL, 
 ) ON ARCHIVE
