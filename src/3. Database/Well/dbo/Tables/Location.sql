﻿CREATE TABLE [dbo].[Location]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[BranchId] INT NOT NULL,
	[AccountCode] VARCHAR(20) NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	[AddressLine1] VARCHAR(50) NOT NULL,
	[AddressLine2] VARCHAR(50) NOT NULL,
	[Postcode] VARCHAR(10) NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Location_Branch] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Branch] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION

)
