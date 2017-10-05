CREATE TABLE [archive].[Account]
(
	[Id] INT NOT NULL,
	[Code] VARCHAR(20) NOT NULL,
	[AccountTypeCode] VARCHAR(20) NOT NULL,
	[DepotId] INT  NULL,
	[Name] VARCHAR(50) NOT NULL,
	[Address1] VARCHAR(50) NOT NULL,
	[Address2] VARCHAR(50) NOT NULL,
	[PostCode]	VARCHAR(10) NOT NULL,
	[ContactName] VARCHAR(50) NOT NULL,
	[ContactNumber] VARCHAR(15) NOT NULL,
	[ContactNumber2] VARCHAR(15) NULL,
	[ContactEmailAddress] VARCHAR(50) NULL,
	[DateDeleted] DATETIME NULL, 
	[StopId] INT NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	[LocationId] INT NULL,
	[ArchiveDate] SMALLDATETIME NOT NULL
) ON Archive
