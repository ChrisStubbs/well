﻿CREATE TABLE [dbo].[User]
(
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Name] VARCHAR(255) NOT NULL,
	[IdentityName] VARCHAR(255) NOT NULL,
	[JobDescription] VARCHAR(500) NOT NULL,
	[Domain] VARCHAR(50) NOT NULL,
	[ThresholdLevelId] INT NULL,
	[CreatedBy] VARCHAR(50) NOT NULL,
	[DateCreated] DATETIME NOT NULL,
	[UpdatedBy] VARCHAR(50) NOT NULL,
	[DateUpdated] DATETIME NOT NULL,
	[Version] [TIMESTAMP] NOT NULL,
	CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [IX_IdentityName] UNIQUE NONCLUSTERED ([IdentityName] ASC),
	CONSTRAINT [FK_ThresholdLevelId_ThesholdLevel] FOREIGN KEY ([ThresholdLevelId]) REFERENCES [dbo].[ThresholdLevel] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION
)
