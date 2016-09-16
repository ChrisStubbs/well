﻿CREATE TABLE [dbo].[SeasonalDate]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[BranchId] INT NOT NULL,
	[Description] VARCHAR(255) NOT NULL,
	[From] DateTime NOT NULL,
	[To] DateTime NOT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[CreatedDate] DATETIME NOT NULL,
	[LastUpdatedBy] VARCHAR(50) NOT NULL,
	[LastUpdatedDate] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
    CONSTRAINT [PK_SeasonalDate] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_SeasonalDate_Branch] FOREIGN KEY ([BranchId]) REFERENCES [dbo].[Branch] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
